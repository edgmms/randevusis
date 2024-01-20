using Hyper.Core;
using Hyper.Core.Configuration;
using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Data;
using Hyper.Services.Security;
using System;
using System.Globalization;
using System.Linq;

namespace Hyper.Services.ApplicationUsers
{
    public partial class ApplicationUserService : BaseService<ApplicationUser>, IApplicationUserService
    {
        private readonly ApplicationUserSettings _applicationUserSettings;
        private readonly IRepository<ApplicationUser> _applicationUserRepository;
        private readonly IRepository<ApplicationUserPassword> _applicationUserPasswordRepository;
        private readonly IRepository<ApplicationUserRole> _applicationUserRoleRepository;
        private readonly IRepository<ApplicationUserApplicationUserRoleMapping> _applicationUserApplicationUserRoleMappingRepository;
        private readonly IEncryptionService _encryptionService;

        public ApplicationUserService(ApplicationUserSettings applicationUserSettings,
            IRepository<ApplicationUser> applicationUserRepository,
            IRepository<ApplicationUserPassword> applicationUserPasswordRepository,
            IRepository<ApplicationUserRole> applicationUserRoleRepository,
            IRepository<ApplicationUserApplicationUserRoleMapping> applicationUserApplicationUserRoleMappingRepository,
            IEncryptionService encryptionService) : base(applicationUserRepository)
        {
            _applicationUserSettings = applicationUserSettings;
            _applicationUserRepository = applicationUserRepository;
            _applicationUserPasswordRepository = applicationUserPasswordRepository;
            _applicationUserRoleRepository = applicationUserRoleRepository;
            _applicationUserApplicationUserRoleMappingRepository = applicationUserApplicationUserRoleMappingRepository;
            _encryptionService = encryptionService;
        }

        #region Application Users

        public virtual ApplicationUser GetApplicationUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"'{nameof(email)}' cannot be null or empty.", nameof(email));

            return _applicationUserRepository.Table.FirstOrDefault(x => x.Email == email);
        }

        public virtual ApplicationUserLoginResults LoginRequest(string usernameOrEmail, string password)
        {
            var applicationUser = this.GetApplicationUserByEmail(usernameOrEmail);
            if (applicationUser is null)
                return ApplicationUserLoginResults.ApplicationUserNotExist;

            if (applicationUser.Deleted)
                return ApplicationUserLoginResults.Deleted;

            if (!applicationUser.Active)
                return ApplicationUserLoginResults.NotActive;

            if (applicationUser.CannotLoginUntilDateUtc.HasValue && applicationUser.CannotLoginUntilDateUtc.Value > DateTime.UtcNow)
                return ApplicationUserLoginResults.LockedOut;

            if (!PasswordsMatch(this.GetCurrentPassword(applicationUser.Id), password))
            {
                //wrong password
                applicationUser.FailedLoginAttempts++;
                if (_applicationUserSettings.FailedPasswordAllowedAttempts > 0 &&
                    applicationUser.FailedLoginAttempts >= _applicationUserSettings.FailedPasswordAllowedAttempts)
                {
                    //lock out
                    applicationUser.CannotLoginUntilDateUtc = DateTime.UtcNow.AddMinutes(_applicationUserSettings.FailedPasswordLockoutMinutes);
                    //reset the counter
                    applicationUser.FailedLoginAttempts = 0;
                }

                _applicationUserRepository.Update(applicationUser);
                return ApplicationUserLoginResults.WrongPassword;
            }

            applicationUser.LastLoginDateUtc = DateTime.UtcNow;
            _applicationUserRepository.Update(applicationUser);

            return ApplicationUserLoginResults.Successful;
        }

        public virtual ApplicationUserRegistrationResult RegisterRequest(ApplicationUserRegistrationRequest request)
        {
            var result = new ApplicationUserRegistrationResult();

            var applicationUser = this.GetApplicationUserByEmail(request.UsernameOrEmail);
            if (applicationUser is not null)
            {
                result.AddError("Bu email'e ait mevcut bir hesabınız bulunmaktadır.");
                return result;
            }

            if (!CommonHelper.IsValidEmail(request.UsernameOrEmail))
            {
                result.AddError("Geçerli bir email adresi giriniz");
                return result;
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                result.AddError("Şifre boş olamaz");
                return result;
            }

            applicationUser = new ApplicationUser
            {
                Email = request.UsernameOrEmail,
                Username = request.UsernameOrEmail,
                Active = true,
                Deleted = false,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                IsSystemAccount = request.IsSystemAccount,
                RequireReLogin = request.RequireReLogin,
                LastActivityDateUtc = DateTime.UtcNow,
                SystemName = request.SystemName,
                RegisteredInStoreId = request.RegisteredInStoreId,
            };

            _applicationUserRepository.Insert(applicationUser);

            var applicationUserPassword = new ApplicationUserPassword
            {
                PasswordFormat = request.PasswordFormat,
                Active = true,
                ApplicationUserId = applicationUser.Id,
                CreatedOnUtc = DateTime.UtcNow,
                PasswordFormatId = (int)request.PasswordFormat
            };

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    applicationUserPassword.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    applicationUserPassword.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    var saltKey = _encryptionService.CreateSaltKey(_applicationUserSettings.PasswordSaltKeySize);
                    applicationUserPassword.PasswordSalt = saltKey;
                    applicationUserPassword.Password = _encryptionService.CreatePasswordHash(request.Password, saltKey, _applicationUserSettings.HashedPasswordFormat);
                    break;
            }

            _applicationUserPasswordRepository.Insert(applicationUserPassword);

            //add to 'Registered' role
            var customerRole = this.GetApplicationUserRoleBySystemName(ApplicationUserDefaults.CustomersRoleName);
            if (customerRole == null)
                throw new Exception($"'{ApplicationUserDefaults.CustomersRoleName}' role could not be loaded");

            var mapping = new ApplicationUserApplicationUserRoleMapping
            {
                ApplicationUserId = applicationUser.Id,
                ApplicationUserRoleId = customerRole.Id,
            };

            _applicationUserApplicationUserRoleMappingRepository.Insert(mapping);

            return result;
        }



        #endregion

        #region Application User Roles

        public virtual ApplicationUserRole GetApplicationUserRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                throw new ArgumentException($"'{nameof(systemName)}' cannot be null or whitespace.", nameof(systemName));

            return _applicationUserRoleRepository.Table.FirstOrDefault(x => x.SystemName == systemName);
        }

        #endregion

        #region Password

        protected bool PasswordsMatch(ApplicationUserPassword applicationUserPassword, string enteredPassword)
        {
            if (applicationUserPassword == null || string.IsNullOrEmpty(enteredPassword))
                return false;

            var savedPassword = string.Empty;
            switch (applicationUserPassword.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    savedPassword = enteredPassword;
                    break;
                case PasswordFormat.Encrypted:
                    savedPassword = _encryptionService.EncryptText(enteredPassword);
                    break;
                case PasswordFormat.Hashed:
                    savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, applicationUserPassword.PasswordSalt, _applicationUserSettings.HashedPasswordFormat);
                    break;
            }

            if (applicationUserPassword.Password == null)
                return false;

            return applicationUserPassword.Password.Equals(savedPassword);
        }

        public virtual ApplicationUserPassword GetCurrentPassword(int applicationUserId)
        {
            var currentPassword = _applicationUserPasswordRepository.Table
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.ApplicationUserId == applicationUserId);

            return currentPassword;
        }

        public virtual void ChangePassword(ApplicationUser applicationUser, string password)
        {
            if (applicationUser is null)
                throw new ArgumentNullException(nameof(applicationUser));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            //delete old records 
            var applicationUserPasswords = _applicationUserPasswordRepository.Table.Where(x => x.Id == applicationUser.Id && !x.Deleted).ToList();
            foreach (var applicationUserPassword in applicationUserPasswords)
            {
                applicationUserPassword.Deleted = true;
                applicationUserPassword.Active = true;
            }
            _applicationUserPasswordRepository.UpdateRange(applicationUserPasswords);

            //create new password
            var saltKey = _encryptionService.CreateSaltKey(_applicationUserSettings.PasswordSaltKeySize);
            var newPassword = new ApplicationUserPassword
            {
                Active = true,
                Deleted = false,
                ApplicationUserId = applicationUser.Id,
                PasswordFormat = PasswordFormat.Hashed,
                CreatedOnUtc = DateTime.UtcNow,
                PasswordSalt = saltKey,
                Password = _encryptionService.CreatePasswordHash(password, saltKey, _applicationUserSettings.HashedPasswordFormat)
            };
            _applicationUserPasswordRepository.Insert(newPassword);
        }

        public void SendPasswordRecoveryToken(ApplicationUser applicationUser)
        {
            var saltKey = _encryptionService.CreateSaltKey(_applicationUserSettings.PasswordSaltKeySize);
            applicationUser.PasswordRecoveryToken = Guid.NewGuid().ToString();
            applicationUser.PasswordRecoveryTokenCreatedOnUtc = DateTime.UtcNow;
            applicationUser.PasswordRecoveryTokenSaltKey = saltKey;
            applicationUser.PasswordRecoveryHashedToken = _encryptionService.CreatePasswordHash(applicationUser.PasswordRecoveryToken, saltKey, _applicationUserSettings.HashedPasswordFormat);
            _applicationUserRepository.Update(applicationUser);

            //todo: send email, sms or anything
        }

        public virtual ApplicationUser ValidatePasswordRecoveryToken(string passwordRecoveryHashedToken)
        {
            if (string.IsNullOrEmpty(passwordRecoveryHashedToken))
                throw new ArgumentException($"'{nameof(passwordRecoveryHashedToken)}' cannot be null or empty.", nameof(passwordRecoveryHashedToken));

            var applicationUser = _applicationUserRepository.Table.FirstOrDefault(x => x.PasswordRecoveryHashedToken == passwordRecoveryHashedToken);

            if (applicationUser is not null && applicationUser.PasswordRecoveryTokenCreatedOnUtc.HasValue
                && applicationUser.PasswordRecoveryTokenCreatedOnUtc.Value.AddHours(1) < DateTime.UtcNow)
            {
                var hashedToken = _encryptionService.CreatePasswordHash(applicationUser.PasswordRecoveryToken, applicationUser.PasswordRecoveryTokenSaltKey, _applicationUserSettings.HashedPasswordFormat);
                if (hashedToken == passwordRecoveryHashedToken)
                    return applicationUser;
            }

            return null;
        }

        #endregion
    }
}