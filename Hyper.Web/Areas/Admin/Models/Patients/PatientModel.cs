using Hyper.Web.Infrastructure.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Patients
{
    public class PatientModel : BaseEntityModel<int>
    {
        [Display(Name = "T.C. Kimlik Numarası")]
        public string TurkishIdentityNumber { get; set; }

        [Display(Name = "İsim")]
        public string Name { get; set; }

        [Display(Name = "Soyisim")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Email Bildirimleri Aktif")]
        public bool SendEmailNotifications { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Sms Bildirimleri Aktif")]
        public bool SendSmsNotifications { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }
    }
}
