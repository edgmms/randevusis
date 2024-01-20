using FluentValidation;
using Hyper.Core.Extensions;
using Hyper.Web.Infrastructure.Validation;
using Hyper.Web.Models.ApplicationUsers;

namespace Hyper.Web.Validations.ApplicationUsers
{
    public class RegisterValidator : BaseValidation<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.UsernameOrEmail)
                    .NotEmpty()
                    .WithMessage("Email adresinizi giriniz.")
                    .EmailAddress()
                    .WithMessage("Lütfen geçerli bir email adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Şifrenizi giriniz.");

            RuleFor(x => x.TurkishIdentityNumber)
               .NotEmpty()
               .WithMessage("T.C. kimlik numaranızı giriniz.");

            RuleFor(x => x.FirstName)
               .NotEmpty()
               .WithMessage("Adınızı giriniz.");

            RuleFor(x => x.LastName)
               .NotEmpty()
               .WithMessage("Soyadınızı giriniz.");

            RuleFor(x => x.Phone)
               .NotEmpty()
               .WithMessage("Telefon numaranızı giriniz.");

            //RuleFor(x => x.StoreAddress)
            //    .NotEmpty()
            //    .WithMessage("İşletme adresinizi giriniz.");

            RuleFor(x => x.StoreEmail)
                //.NotEmpty()
                //.WithMessage("İşletme mailinizi giriniz.")
                .EmailAddress()
                .WithMessage("Lütfen geçerli bir email adresi giriniz");

            //RuleFor(x => x.StoreName)
            //    .NotEmpty()
            //    .WithMessage("İşletme Adını giriniz.");

            //RuleFor(x => x.StorePhone)
            //    .NotEmpty()
            //    .WithMessage("İşletme Telefonunu giriniz.");

            //RuleFor(x => x.StoreTaxAdministration)
            //    .NotEmpty()
            //    .WithMessage("İşletme Vergi Dairesisini giriniz.");

            //RuleFor(x => x.StoreTaxNumber)
            //    .NotEmpty()
            //    .WithMessage("İşletme Vergi Numarasını giriiniz.");

        }
    }
}
