using Hyper.Core.Domain.Appointments;
using Hyper.Core.Domain.Products;
using Hyper.Web.Areas.Admin.Models.Appointments;
using System;

namespace Hyper.Web.Areas.Admin.Factories.Appointments
{
    public interface IAppointmentFactory
    {
        Appointment PrepareAppointment(AppointmentCreateModel model, Product product, DateTime startDateTime);
        Appointment PrepareAppointment(AppointmentEditModel model, Appointment entity, Product product, DateTime startDateTime);
        AppointmentCreateModel PrepareAppointmentCreateModel(AppointmentCreateModel model);
        AppointmentEditModel PrepareAppointmentEditModel(Appointment entity);
        string PrepareColor(int paymentTypeId, int paymentStatusId, int appointmentStatusId);
    }
}
