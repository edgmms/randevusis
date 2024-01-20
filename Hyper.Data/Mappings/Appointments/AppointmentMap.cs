using Hyper.Core.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hyper.Data.Mappings.Appointments
{
    public class AppointmentMap : HyperEntityTypeConfigurator<Appointment>
    {
        public override void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable(nameof(Appointment));
            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.PaymentType);
            builder.Ignore(x => x.PaymentStatus);
            builder.Ignore(x => x.AppointmentStatus);
        }
    }
}
