using Hyper.Core.Domain.ApplicationUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.ApplicationUsers
{
    public partial class ApplicationUserPasswordMap : HyperEntityTypeConfigurator<ApplicationUserPassword>
    {
        public override void Configure(EntityTypeBuilder<ApplicationUserPassword> builder)
        {
            builder.ToTable(nameof(ApplicationUserPassword));
            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.PasswordFormat);
        }
    }
}
