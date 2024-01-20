using Hyper.Core;
using Hyper.Core.Domain.Products;
using Hyper.Core.Extensions;
using Hyper.Data;
using System.Linq;

namespace Hyper.Services.Products
{
    public partial class ProductService : BaseService<Product>, IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IWorkContext _workContext;

        public ProductService(IRepository<Product> productRepository, IWorkContext workContext) : base(productRepository)
        {
            _productRepository = productRepository;
            _workContext = workContext;
        }

        public IPagedList<Product> SearchProducts(string name = "", ProductType? productType = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productRepository.Table;

            if (!name.IsNullOrEmpty())
                query = query.Where(x => x.Name.Contains(name));

            if (productType is not null)
                query = query.Where(x => x.ProductTypeId == (int)productType);

            return new PagedList<Product>(query, pageIndex, pageSize);
        }
    }
}
