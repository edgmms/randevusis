using Hyper.Core.Domain.ApplicationUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.ApplicationUsers
{
    public partial class ApplicationUserMap : HyperEntityTypeConfigurator<ApplicationUser>
    {
        public override void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(nameof(ApplicationUser));
            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.FullName);
        }
    }
}
