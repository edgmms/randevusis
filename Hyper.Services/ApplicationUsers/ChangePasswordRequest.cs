using Hyper.Core.Domain.ApplicationUsers;

namespace Hyper.Services.ApplicationUsers
{
    public class ChangePasswordRequest
    {
        public PasswordFormat PasswordFormat { get; set; }
        public string UsernameOrEmail { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
