using Hyper.Core.Domain.ApplicationUsers;

namespace Hyper.Services.ApplicationUsers
{
    public partial interface IApplicationUserService : IBaseService<ApplicationUser>
    {
        ApplicationUser GetApplicationUserByEmail(string email);
        ApplicationUserLoginResults LoginRequest(string usernameOrEmail, string password);
        ApplicationUserRegistrationResult RegisterRequest(ApplicationUserRegistrationRequest request);
        ApplicationUserRole GetApplicationUserRoleBySystemName(string systemName);
        ApplicationUserPassword GetCurrentPassword(int applicationUserId);
        void ChangePassword(ApplicationUser applicationUser, string password);
        void SendPasswordRecoveryToken(ApplicationUser applicationUser);
        ApplicationUser ValidatePasswordRecoveryToken(string passwordRecoveryHashedToken);
	}
}