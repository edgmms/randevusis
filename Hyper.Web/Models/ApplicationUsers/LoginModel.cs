using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Models.ApplicationUsers
{
    public class LoginModel
    {
        [Display(Name ="Email")]
        public string UsernameOrEmail { get; set; }

        [Display(Name ="Şifre")]
        public string Password { get; set; }

        [Display(Name ="Bilgileri Hatırla")]
		public bool RememberMe { get; set; }
    }
}