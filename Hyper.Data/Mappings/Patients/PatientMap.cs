using Hyper.Core.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Patients
{
    public partial class PatientMap : HyperEntityTypeConfigurator<Patient>
    {
        public override void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable(nameof(Patient));
            builder.HasKey(e => e.Id);

            builder.Ignore(e => e.FullName);
        }
    }
}
