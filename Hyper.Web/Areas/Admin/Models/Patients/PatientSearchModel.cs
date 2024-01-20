using Hyper.Web.Areas.Admin.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Patients
{
    public class PatientSearchModel : BaseSearchModel
    {
        [Display(Name = "İsim")]
        public string SearchName { get; set; }

        [Display(Name = "Soyisim")]
        public string SearchSurname { get; set; }

        [Display(Name = "T.C. Kimlik")]
        public string SearchTurkishIdentityNumber { get; set; }

        [Display(Name = "Email")]
        public string SearchEmail { get; set; }

        [Display(Name = "Email Bildirim Aktif")]
        public bool SearchSendEmailNotifications { get; set; }

        [Display(Name = "Telefon")]
        public string SearchPhone { get; set; }

        [Display(Name = "Sms Bildirim Aktif")]
        public bool SearchSendSmsNotifications { get; set; }

        [Display(Name = "Adres")]
        public string SearchAddress { get; set; }

        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }
    }
}
