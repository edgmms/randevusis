using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Hyper.Services.Appointments
{
    public partial interface IAppointmentService : IBaseService<Appointment>
    {
        IPagedList<Appointment> SearchAppointments(DateTime? startDateTimeFrom = null, DateTime? startDateTimeTo = null, DateTime? endDateTime = null, int pageIndex = 0, int pageSize = int.MaxValue);

        List<AppointmentCalendarItem> SearchAppointments(string patientName = "", string patientSurname = "", DateTime? startDateTime = null, DateTime? endDateTime = null);
    }
}
