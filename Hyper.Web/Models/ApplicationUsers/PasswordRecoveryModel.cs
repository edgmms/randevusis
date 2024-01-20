using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Models.ApplicationUsers
{
	public class PasswordRecoveryModel
	{
		[Display(Name = "Email adresinizi giriniz")]
		public string UsernameOrEmail { get; set; }

		public bool Success { get; set; }

	}
}
