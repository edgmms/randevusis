namespace Hyper.Web.Areas.Admin.Models.ApplicationUsers
{
    public class ChangePasswordModel 
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
