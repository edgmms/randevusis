using Hyper.Core.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Products
{
    public partial class ProductMap : HyperEntityTypeConfigurator<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(e => e.Id);

            builder.Ignore(x => x.ProductType);
        }
    }
}