using Hyper.Core.Domain.ApplicationUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.ApplicationUsers
{
    public partial class ApplicationUserApplicationUserRoleMappingMap : HyperEntityTypeConfigurator<ApplicationUserApplicationUserRoleMapping>
    {
        public override void Configure(EntityTypeBuilder<ApplicationUserApplicationUserRoleMapping> builder)
        {
            builder.ToTable(nameof(ApplicationUserApplicationUserRoleMapping));
            builder.HasKey(x => x.Id);
        }
    }
}
