using FluentValidation;
using Hyper.Web.Areas.Admin.Models.Appointments;
using Hyper.Web.Infrastructure.Validation;

namespace Hyper.Web.Areas.Admin.Validations.Appointments
{
    public class AppointmentCreateValidator : BaseValidation<AppointmentCreateModel>
    {
        public AppointmentCreateValidator()
        {
            RuleFor(x => x.AssignedEmployeeId)
            .NotEmpty()
            .WithMessage("Lütfen personel seçiniz")
            .GreaterThan(0)
            .WithMessage("Lütfen personel seçiniz");

            RuleFor(x => x.AssignedPatientId)
            .NotEmpty()
            .WithMessage("Lütfen danışan seçiniz")
            .GreaterThan(0)
            .WithMessage("Lütfen danışan seçiniz");

            RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Lütfen başlangıç tarihi seçiniz");

            RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Lütfen başlangıç saati seçiniz");

            RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Lütfen ürün veya hizmet seçiniz")
            .GreaterThan(0)
            .WithMessage("Lütfen ürün veya hizmet seçiniz");

            RuleFor(x => x.DiscountTotal)
           .GreaterThanOrEqualTo(0)
           .WithMessage("İndirim tutarı 0'dan küçük olamaz");
        }
    }
}