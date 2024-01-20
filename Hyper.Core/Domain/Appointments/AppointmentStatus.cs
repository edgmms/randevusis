using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hyper.Core.Domain.Appointments
{
    public enum AppointmentStatus
    {
        [Description("Bekliyor")]
        Pending = 0,

        [Description("İptal Edildi")]
        Canceled = 10,

        [Description("Ertelendi")]
        Postponed = 30,

        [Description("Danışan Onayı Alındı")]
        PatientConfirmed = 40,

        [Description("Tamamlandı")]
        Completed = 80,
    }
}
