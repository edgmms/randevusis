using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Appointments
{
    public class AppointmentEditModel
    {
        public AppointmentEditModel()
        {
            this.AvailableProducts = new List<SelectListItem>();
            this.AvailableEmployees = new List<SelectListItem>();
            this.AvailableDurations = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Display(Name = "Randevu Tarihi")]
        public string StartDate { get; set; }

        [Display(Name = "Randevu Saati")]
        public string StartTime { get; set; }

        [Display(Name = "Randevu Süresi")]
        public int DurationAsMinute { get; set; }
        public List<SelectListItem> AvailableDurations { get; set; }

        [Display(Name = "Ürün | Hizmet")]
        public int ProductId { get; set; }
        public List<SelectListItem> AvailableProducts { get; set; }

        [Display(Name = "İndirim Tutarı")]
        public decimal DiscountTotal { get; set; }

        [Display(Name = "Danışan")]
        public string AssingedPatientText { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal TotalPrice { get; set; }

        public string AssingedPatientTurkishIdentityNumber { get; set; }

        [Display(Name = "Personel")]
        public int AssignedEmployeeId { get; set; }
        public List<SelectListItem> AvailableEmployees { get; set; }

        [Display(Name = "Ödeme Türü")]
        public int PaymentTypeId { get; set; }
        public SelectList AvailablePaymentTypes { get; set; }

        [Display(Name = "Ödeme Durumu")]
        public int PaymentStatusId { get; set; }
        public SelectList AvailablePaymentStatuses { get; set; }

        [Display(Name = "Randevu Durumu")]
        public int AppointmentStatusId { get; set; }
        public SelectList AvailableAppointmentStatuses { get; set; }

        public string Color { get; set; }

        public bool IsStoreAdministrator { get; set; }

        [Display(Name = "Not")]
        public string Note { get;  set; }
    }
}