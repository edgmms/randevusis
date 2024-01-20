using Hyper.Core.Domain.ApplicationUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.ApplicationUsers
{
    public partial class ApplicationUserRoleMap : HyperEntityTypeConfigurator<ApplicationUserRole>
    {
        public override void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable(nameof(ApplicationUserRole));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.SystemName).HasMaxLength(255).IsRequired();
        }
    }
}