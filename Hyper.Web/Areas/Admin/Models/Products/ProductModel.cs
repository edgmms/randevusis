using Hyper.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Products
{
    public class ProductModel : BaseEntityModel<int>
    {
        [Display(Name ="Adı")]
        public string Name { get; set; }
        
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        
        [Display(Name ="Tipi")]
        public int ProductTypeId { get; set; }
        
        [Display(Name ="Fiyatı")]
        public decimal Price { get; set; }
        
        [Display(Name = "Eski Fiyatı")]
        public decimal OldPrice { get; set; }
        
        [Display(Name = "Maliyeti")]
        public decimal ProductCost { get; set; }

        [Display(Name = "Vergi Oranı")]
        public int TaxRate { get; set; }

        public List<SelectListItem> AvailableProductTypes { get; set; }

    }
}
