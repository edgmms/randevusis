using Hyper.Core.Domain.Organisations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Organisations
{
    public class StoreMap : HyperEntityTypeConfigurator<Store>
    {
        public override void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable(nameof(Store));
            builder.HasKey(e => e.Id);
        }
    }
}
