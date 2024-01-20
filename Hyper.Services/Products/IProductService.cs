using Hyper.Core;
using Hyper.Core.Domain.Products;

namespace Hyper.Services.Products
{
    public partial interface IProductService : IBaseService<Product>
    {
        IPagedList<Product> SearchProducts(string name = "", ProductType? productType = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}