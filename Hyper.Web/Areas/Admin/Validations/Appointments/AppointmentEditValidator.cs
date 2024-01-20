using FluentValidation;
using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using Hyper.Web.Areas.Admin.Models.Appointments;
using Hyper.Web.Infrastructure.Validation;

namespace Hyper.Web.Areas.Admin.Validations.Appointments
{
    public class AppointmentEditValidator : BaseValidation<AppointmentEditModel>
    {
        public AppointmentEditValidator(IWorkContext workContext)
        {
            RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Lütfen randevu tarihi seçiniz");

            RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Lütfen randevu saati seçiniz");

            RuleFor(x => x.DurationAsMinute)
            .NotEmpty()
            .WithMessage("Lütfen randevu süresi seçiniz")
            .GreaterThan(0)
            .WithMessage("Lütfen randevu süresi seçiniz");

            RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Lütfen ürün veya hizmet seçiniz")
            .GreaterThan(0)
            .WithMessage("Lütfen ürün veya hizmet seçiniz");

            RuleFor(x => x.DiscountTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("İndirim tutarı 0'dan küçük olamaz");


            if (workContext.IsStoreAdministrator)
            {
                RuleFor(x => x.AssignedEmployeeId)
                 .NotEmpty()
                 .WithMessage("Lütfen personel seçiniz")
                 .GreaterThan(0)
                 .WithMessage("Lütfen personel seçiniz");
            }

            RuleFor(x => x.AppointmentStatusId)
            .Must(appointmentStatusId =>
            {
                try
                {
                    var appointmentStatus = (AppointmentStatus)appointmentStatusId;
                    return true;
                }
                catch
                {
                    return false;
                }
            }).WithMessage("Lütfen randevu durumu seçiniz");

            RuleFor(x => x.PaymentStatusId)
            .Must(paymentStatusId =>
            {
                try
                {
                    var paymentStatus = (PaymentStatus)paymentStatusId;
                    return true;
                }
                catch
                {
                    return false;
                }
            }).WithMessage("Lütfen ödeme durumu seçiniz");

            RuleFor(x => x.PaymentTypeId)
            .Must(paymentTypeId =>
            {
                try
                {
                    var paymentType = (PaymentType)paymentTypeId;
                    return true;
                }
                catch
                {
                    return false;
                }
            }).WithMessage("Lütfen ödeme tipi seçiniz");
        }
    }
}
