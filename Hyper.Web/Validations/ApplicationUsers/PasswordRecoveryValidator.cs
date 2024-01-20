using FluentValidation;
using Hyper.Web.Infrastructure.Validation;
using Hyper.Web.Models.ApplicationUsers;

namespace Hyper.Web.Validations.ApplicationUsers
{
	public class PasswordRecoveryValidator : BaseValidation<PasswordRecoveryModel>
	{
		public PasswordRecoveryValidator()
		{
			RuleFor(x => x.UsernameOrEmail)
			.NotEmpty()
			.WithMessage("Email adresinizi giriniz.")
			.EmailAddress()
			.WithMessage("Lütfen geçerli bir email adresi giriniz.");
		}
	}
}
