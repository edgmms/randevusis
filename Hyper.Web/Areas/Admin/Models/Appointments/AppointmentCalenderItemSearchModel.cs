using System;

namespace Hyper.Web.Areas.Admin.Models.Appointments
{
    public class AppointmentCalenderItemSearchModel
    {
        public string PatientName { get; set; }
        public string PatientSurname { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
