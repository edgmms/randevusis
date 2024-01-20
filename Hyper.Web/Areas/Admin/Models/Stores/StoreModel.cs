using Hyper.Web.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Stores
{
    public class StoreModel : BaseEntityModel<int>
    {
        [Display(Name = "İşletme Adı")]
        public string Name { get; set; }

        [Display(Name = "İşletme Maili")]
        public string Email { get; set; }

        [Display(Name = "İşletme Telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "İşletme Adresi")]
        public string Address { get; set; }

        [Display(Name = "Vergi Dairesi")]
        public string TaxAdministration { get; set; }

        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }

        [Display(Name = "Randevu Durumu » Bekliyor")]
        public string AppointmentStatusPendingColor { get; set; }

        [Display(Name = "Randevu Durumu » İptal")]
        public string AppointmentStatusCanceledColor { get; set; }

        [Display(Name = "Randevu Durumu » Ertelendi")]
        public string AppointmentStatusPostponedColor { get; set; }

        [Display(Name = "Randevu Durumu » Danışan Onayı Alındı")]
        public string AppointmentStatusPatientConfirmedColor { get; set; }

        [Display(Name = "Randevu Durumu » Tamamlandı")]
        public string AppointmentStatusCompletedColor { get; set; }

        [Display(Name = "Ödeme Durumu » Bekliyor")]
        public string PaymentStatusPendingColor { get; set; }

        [Display(Name = "Ödeme Durumu » Tamamlandı")]
        public string PaymentStatusCompletedColor { get; set; }

        [Display(Name = "Ödeme Tipi » Nakit")]
        public string PaymentTypeCashColor { get; set; }

        [Display(Name = "Ödeme Tipi » Kredi Kartı")]
        public string PaymentTypeCreditCardColor { get; set; }

        [Display(Name = "Mesai Başlangıç Saati")]
        public string BusinessStartTime { get; set; }

        [Display(Name = "Mesai Bitiş Saati")]
        public string BusinessEndTime { get; set; }

        [Display(Name = "Cumartesi Tatil Mi ?")]
        public bool SaturdayHoliday { get; set; }
        
        [Display(Name = "Pazar Tatil Mi ?")]
        public bool SundayHoliday { get; set; }

        [Display(Name = "Sms Entegrasyon Kullanıcı Adı")]
        public string  SmsUsername { get; set; }

        [Display(Name = "Sms Entegrasyon Şifresi")]
        public string  SmsPassword { get; set; }

        [Display(Name = "Sms Başlığı")]
        public string SmsHeader { get; set; }
    }
}
