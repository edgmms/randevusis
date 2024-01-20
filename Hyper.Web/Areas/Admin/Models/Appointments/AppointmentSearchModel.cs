using Hyper.Web.Areas.Admin.Infrastructure.Models;
using System;

namespace Hyper.Web.Areas.Admin.Models.Appointments
{
    public class AppointmentSearchModel : BaseSearchModel
    {
        public DateTime SearchStartDateTime { get; set; }
        public DateTime SearchEndDateTime { get; set; }

        public string BusinessStartTime { get; set; }
        public string BusinessEndTime { get; set; }
        public bool SaturdayHoliday { get; set; }
        public bool SundayHoliday { get; set; }
    }
}
