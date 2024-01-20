using Hyper.Core.Domain.Organisations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Organisations
{
    public class BranchMap : HyperEntityTypeConfigurator<Branch>
    {
        public override void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable(nameof(Branch));
            builder.HasKey(e => e.Id);
        }
    }
}
