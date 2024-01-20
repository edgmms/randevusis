using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Models.ApplicationUsers
{
    public class RegisterModel
    {
        [Display(Name ="Ad")]
        public string FirstName { get; set; }

        [Display(Name ="Soyad")]
        public string LastName { get; set; }

        [Display(Name ="Email")]
        public string UsernameOrEmail { get; set; }

        [Display(Name ="Şifre")]
        public string Password { get; set; }

        [Display(Name ="Telefon")]
        public string Phone { get; set; }

        [Display(Name ="T.C. Kimlik Numarası")]
        public string TurkishIdentityNumber { get; set; }

        [Display(Name ="İşletme Adı")]
        public string StoreName { get; set; }

        [Display(Name = "İşletme Maili")]
        public string StoreEmail { get; set; }

        [Display(Name = "İşletme Telefonu")]
        public string StorePhone { get; set; }

        [Display(Name = "İşletme Adresi")]
        public string StoreAddress { get; set; }

        [Display(Name = "Vergi Dairesi")]
        public string StoreTaxAdministration { get; set; }

        [Display(Name = "Vergi Numarası")]
        public string StoreTaxNumber { get; set; }

        [Display(Name ="Kullanım Koşulları")]
        public bool TermOfServices { get; set; }
    }
}