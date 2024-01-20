using Hyper.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Appointments
{
    public class AppointmentCreateModel
    {
        public AppointmentCreateModel()
        {
            this.AvailableProducts = new List<SelectListItem>();
            this.AvailableEmployees = new List<SelectListItem>();
            this.AvailablePatients = new List<SelectListItem>();
            this.AvailableDurations = new List<SelectListItem>();
        }

        [Display(Name = "Randevu Tarihi")]
        public string StartDate { get; set; }

        [Display(Name = "Randevu Saati")]
        public string StartTime { get; set; }

        [Display(Name = "Randevu Süresi")]
        public int DurationAsMinute { get; set; }
        public List<SelectListItem> AvailableDurations { get; set; }

        [Display(Name = "Tekrarlacak mı?")]
        public int PeriodTypeId { get; set; }
        public SelectList AvailablePeriodTypes { get; set; }

        [Display(Name = "Terkar Sayısı")]
        public int PeriodCount { get; set; }

        [Display(Name = "Ürün | Hizmet")]
        public int ProductId { get; set; }
        public List<SelectListItem> AvailableProducts { get; set; }

        [Display(Name = "İndirim Tutarı")]
        public decimal DiscountTotal { get; set; }

        [Display(Name = "Danışan")]
        public int AssignedPatientId { get; set; }
        public List<SelectListItem> AvailablePatients { get; set; }

        [Display(Name = "Personel")]
        public int AssignedEmployeeId { get; set; }
        public List<SelectListItem> AvailableEmployees { get; set; }

        public bool SendSmsToPatient { get; set; }

        [Display(Name = "Not")]
        public string Note { get; set; }
    }
}
