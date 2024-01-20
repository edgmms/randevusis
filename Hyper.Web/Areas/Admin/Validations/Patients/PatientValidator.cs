using FluentValidation;
using Hyper.Web.Areas.Admin.Infrastructure.Validation;
using Hyper.Web.Areas.Admin.Models.Patients;
using Hyper.Web.Infrastructure.Validation;

namespace Hyper.Web.Areas.Admin.Validations.Patients
{
    public class PatientValidator : BaseValidation<PatientModel>
    {
        public PatientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("İsim gereklidir.");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Soyisim gereklidir.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Geçerli bir e-posta adresi giriniz");

            RuleFor(x => x.TurkishIdentityNumber)
                //.NotEmpty()
                //.WithMessage("T.C. Kimlik Numarası gereklidir")
                .Length(11)
                .WithMessage("T.C. Kimlik Numarası 11 hane olmalıdır");
                //.Must(x => { return ValidationExtensions.ValidateTurkishIdentityNumber(x); })
                //.WithMessage("Lütfen geçerli bir T.C. Kimlik Numarası giriniz");
        }
    }
}
