using Hyper.Core.Configuration;
using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Core.Domain.Organisations;
using Hyper.Services.ApplicationUsers;
using Hyper.Services.Authentication;
using Hyper.Services.Notifications;
using Hyper.Services.Organisations;
using Hyper.Web.Models.ApplicationUsers;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Controllers
{
    public class ApplicationUserController : BasePublicController
    {
        private readonly ApplicationUserSettings _applicationUserSettings;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStoreService _storeService;
        private readonly IEmailSender _emailSender;

        public ApplicationUserController(
            ApplicationUserSettings applicationUserSettings,
            IApplicationUserService applicationUserService,
            IAuthenticationService authenticationService,
            IStoreService storeService,
            IEmailSender emailSender)
        {
            _applicationUserSettings = applicationUserSettings;
            _applicationUserService = applicationUserService;
            _authenticationService = authenticationService;
            _storeService = storeService;
            _emailSender = emailSender;
        }

        #region Login / logout

        public virtual IActionResult Login()
        {
            var user = _authenticationService.GetAuthenticatedApplicationUser();
            if (user != null)
                return new RedirectToRouteResult("Admin", null);

            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _applicationUserService.LoginRequest(model.UsernameOrEmail, model.Password);

                switch (loginResult)
                {
                    case ApplicationUserLoginResults.Successful:
                        {
                            var applicationUser = _applicationUserService.GetApplicationUserByEmail(model.UsernameOrEmail);
                            _authenticationService.SignIn(applicationUser, model.RememberMe);
                            return new RedirectToRouteResult("Admin", null);
                        }
                    case ApplicationUserLoginResults.ApplicationUserNotExist:
                        ModelState.AddModelError("", "");
                        break;
                    case ApplicationUserLoginResults.Deleted:
                        ModelState.AddModelError("", "");
                        break;
                    case ApplicationUserLoginResults.NotActive:
                        ModelState.AddModelError("", "");
                        break;
                    case ApplicationUserLoginResults.LockedOut:
                        ModelState.AddModelError("", "");
                        break;
                    case ApplicationUserLoginResults.WrongPassword:
                        ModelState.AddModelError("", "");
                        break;
                    default:
                        ModelState.AddModelError("", "");
                        break;
                }
            }

            return View(model);
        }

        public virtual IActionResult Logout()
        {
            //standard logout 
            _authenticationService.SignOut();
            return RedirectToRoute("Homepage");
        }

        #endregion

        #region Register

        public virtual IActionResult Register()
        {
            var currentApplicationUser = _authenticationService.GetAuthenticatedApplicationUser();
            if (currentApplicationUser is not null)
                _authenticationService.SignOut();

            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var store = new Store
                {
                    Name = model.StoreName ?? "Belirtilmedi",
                    Email = model.StoreEmail ?? model.UsernameOrEmail,
                    Address = model.StoreAddress ?? "Belirtilmedi",
                    PhoneNumber = model.StorePhone ?? "0(500)111-22-33",
                    TaxAdministration = model.StoreTaxAdministration ?? "Belirtilmedi",
                    TaxNumber = model.StoreTaxNumber ?? "Belirtilmedi",
                    BusinessStartTime = "08:00",
                    BusinessEndTime = "17:00",
                    AppointmentStatusPendingColor = "#818387",
                    AppointmentStatusCanceledColor = "#ef4444",
                    AppointmentStatusPostponedColor = "#ff48d8",
                    AppointmentStatusPatientConfirmedColor = "#0c83ff",
                    //AppointmentStatusCompletedColor = "#00ab20",
                    PaymentStatusPendingColor = "#f58646",
                    //PaymentStatusCompletedColor = "#00ab20",
                    PaymentTypeCashColor = "#00ab20",
                    PaymentTypeCreditCardColor = "#059669",
                    SaturdayHoliday = false,
                    SundayHoliday = true
                };

                _storeService.Insert(store);

                var registrationRequest = new ApplicationUserRegistrationRequest
                {
                    UsernameOrEmail = model.UsernameOrEmail,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    IsSystemAccount = false,
                    Password = model.Password,
                    PasswordFormat = PasswordFormat.Hashed,
                    SystemName = ApplicationUserSystemNames.StoreAdministrator,
                    RegisteredInStoreId = store.Id,
                };

                var registrationResult = _applicationUserService.RegisterRequest(registrationRequest);

                if (registrationResult.Success)
                    return RedirectToRoute("Homepage");

                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }

        #endregion

        #region Password

        public virtual IActionResult PasswordRecovery()
        {
            var model = new PasswordRecoveryModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var applicationUser = _applicationUserService.GetApplicationUserByEmail(model.UsernameOrEmail);
            if (applicationUser is not null)
            {
                _applicationUserService.SendPasswordRecoveryToken(applicationUser);
                model.Success = true;
            }

            return View(model);
        }

        public virtual IActionResult PasswordRecoveryConfirm(string t)
        {
            if (string.IsNullOrEmpty(t))
                return RedirectToRoute("HomePage");

            var applicationUser = _applicationUserService.ValidatePasswordRecoveryToken(t);

            if (applicationUser is null)
                return RedirectToRoute("HomePage");

            var model = new PasswordRecoveryConfirmModel
            {
                Token = t
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult PasswordRecoveryConfirm(PasswordRecoveryConfirmModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var applicationUser = _applicationUserService.ValidatePasswordRecoveryToken(model.Token);
            if (applicationUser is not null)
            {
                _applicationUserService.ChangePassword(applicationUser, model.Password);
            }

            return RedirectToRoute("HomePage");
        }

        #endregion
    }
}
