using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using Hyper.Core.Domain.Employees;
using Hyper.Core.Domain.Patients;
using Hyper.Core.Domain.Products;
using Hyper.Core.Extensions;
using Hyper.Data;
using Hyper.Services.Appointments.Extensions;
using Hyper.Services.Employees;
using Hyper.Services.Patients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyper.Services.Appointments
{
    public partial class AppointmentService : BaseService<Appointment>, IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Patient> _patientRepository;

        public AppointmentService(IRepository<Appointment> appointmentRepository,
            IRepository<Product> productRepository,
            IRepository<Employee> employeeRepository,
            IRepository<Patient> patientRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _productRepository = productRepository;
            _employeeRepository = employeeRepository;
            _patientRepository = patientRepository;
        }

        public IPagedList<Appointment> SearchAppointments(DateTime? startDateTimeFrom = null, DateTime? startDateTimeTo = null, DateTime? endDateTime = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _appointmentRepository.Table;

            if (startDateTimeFrom is not null)
                query = query.Where(x => x.StartDateTime > startDateTimeFrom);

            if (startDateTimeTo is not null)
                query = query.Where(x => x.StartDateTime < startDateTimeTo);

            if (endDateTime is not null)
                query = query.Where(x => x.EndDateTime > endDateTime);

            var data = new PagedList<Appointment>(query, pageIndex, pageSize);
            return data;
        }

        public List<AppointmentCalendarItem> SearchAppointments(string patientName = "", string patientSurname = "", DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            var query = _appointmentRepository.Table;

            if (startDateTime is not null)
                query = query.Where(x => x.StartDateTime > startDateTime);

            if (endDateTime is not null)
                query = query.Where(x => x.EndDateTime < endDateTime);

            var calendarQuery = (from appointment in query
                                 join product in _productRepository.Table on appointment.ProductId equals product.Id
                                 join employee in _employeeRepository.Table on appointment.AssignedEmployeeId equals employee.Id
                                 join patient in _patientRepository.Table on appointment.AssignedPatientId equals patient.Id
                                 select new
                                 {
                                     Id = appointment.Id,
                                     AssignedPatientId = appointment.AssignedPatientId,
                                     AssignedEmployeeId = appointment.AssignedEmployeeId,
                                     ProductId = appointment.ProductId,
                                     PaymentTypeId = appointment.PaymentTypeId,
                                     PaymentStatusId = appointment.PaymentStatusId,
                                     AppointmentStatusId = appointment.AppointmentStatusId,
                                     StartDateTime = appointment.StartDateTime,
                                     EndDateTime = appointment.EndDateTime,
                                     ProductPrice = appointment.ProductPrice,
                                     DiscountTotal = appointment.DiscountTotal,
                                     DiscountedPrice = appointment.DiscountedPrice,
                                     Note = appointment.Note,
                                     ProductName = product.Name,
                                     PatientName = patient.Name,
                                     PatientSurname = patient.Surname,
                                     PatientFullName = patient.FullName,
                                     PatientPhone = patient.Phone,
                                     PatientAddress = patient.Address,
                                     PatientTurkishIdentityNumber = patient.TurkishIdentityNumber,
                                     EmployeeFullName = employee.FullName,
                                 });

            if (!patientName.IsNullOrEmpty())
                calendarQuery = calendarQuery.Where(x => x.PatientName.Contains(patientName));

            if (!patientSurname.IsNullOrEmpty())
                calendarQuery = calendarQuery.Where(x => x.PatientSurname.Contains(patientSurname));

            var calendar = calendarQuery.ToList();

            List<AppointmentCalendarItem> calendarResult = new();
            foreach (var calendarItem in calendar)
            {
                var html = $"<div class=\"list-group\">";
                html += $"<div class=\"list-group-item text-white\">{calendarItem.PatientFullName}</div>";
                html += $"<div class=\"list-group-item text-white\">{calendarItem.PatientTurkishIdentityNumber}</div>";
                html += $"<div class=\"list-group-item text-white\">{calendarItem.PatientPhone}</div>";
                html += $"<div class=\"list-group-item text-white\">{calendarItem.PatientAddress}</div>";
                html += $"<div class=\"list-group-item text-white\">{calendarItem.ProductName} {calendarItem.DiscountedPrice}</div>";
                html += $"<div class=\"list-group-item text-white\">İndirim Tutarı: {calendarItem.DiscountTotal}</div>";
                html += $"<div class=\"list-group-item text-white\">Not: {calendarItem.Note}</div>";
                html += "</div>";

                var item = new AppointmentCalendarItem
                {
                    Id = calendarItem.Id,
                    Title = $"{calendarItem.PatientFullName} » {calendarItem.EmployeeFullName}",
                    HoverText = html,
                    AppointmentStatusId = calendarItem.AppointmentStatusId,
                    PaymentTypeId = calendarItem.PaymentTypeId,
                    PaymentStatusId = calendarItem.PaymentStatusId,
                    Start = calendarItem.StartDateTime,
                    End = calendarItem.EndDateTime,
                };
                calendarResult.Add(item);
            }

            return calendarResult;
        }


    }
}
