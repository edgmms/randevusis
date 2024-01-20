using Hyper.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings
{
    public abstract partial class HyperEntityTypeConfigurator<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<int>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {

        }
    }
}
