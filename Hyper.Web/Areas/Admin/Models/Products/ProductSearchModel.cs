using Hyper.Web.Areas.Admin.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Web.Areas.Admin.Models.Products
{
    public class ProductSearchModel : BaseSearchModel
    {
        [Display(Name ="Ürün / Hizmet Adı")]
        public string SearchName { get; set; }
    }
}
