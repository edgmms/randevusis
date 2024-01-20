using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.ApplicationUsers
{
    public class ProfileModel
    { 
        [Display(Name = "Email")]
        public string Email { get; set; }
  
        [Display(Name = "Tam Ad")]
        public string FullName { get; set; }
 
        [Display(Name = "Ad")]
        public string FirstName { get; set; }
 
        [Display(Name = "Soyad")]
        public string LastName { get; set; }
        
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }
 
        [Display(Name = "Doğum Tarihi")]
        public string DateOfBirth { get; set; }
 
        [Display(Name = "Adres")]
        public string StreetAddress { get; set; }
  
        [Display(Name = "Posta Kodu")]
        public string ZipPostalCode { get; set; }
 
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
 
        [Display(Name = "Fax")]
        public string Fax { get; set; }
    }
}