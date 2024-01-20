using System.ComponentModel;

namespace Hyper.Core.Domain.Appointments
{
    public enum PaymentType
    {
        [Description("Nakit")]
        Cash = 20,

        [Description("Kredi Kartı")]
        CreditCard = 30
    }
}