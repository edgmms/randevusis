using System.ComponentModel;

namespace Hyper.Core.Domain.Appointments
{
    public enum PaymentStatus
    {
        [Description("Bekliyor")]
        Pending = 0,

        [Description("Tamamlandı")]
        Completed = 20,
    }
}
