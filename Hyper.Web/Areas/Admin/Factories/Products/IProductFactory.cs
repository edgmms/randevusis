using Hyper.Core.Domain.Products;
using Hyper.Web.Areas.Admin.Models.Products;

namespace Hyper.Web.Areas.Admin.Factories.Products
{
    public interface IProductFactory
    {
        ProductModel PrepareProductModel(Product entity, ProductModel model = null);
        ProductModel PrepareProductModel(ProductModel model, Product entity = null);
    }
}
