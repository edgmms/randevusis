using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using Hyper.Core.Extensions;
using Hyper.Core.Results;
using Hyper.Services.Appointments;
using Hyper.Services.Employees;
using Hyper.Services.Messages;
using Hyper.Services.Patients;
using Hyper.Services.Products;
using Hyper.Web.Areas.Admin.Factories.Appointments;
using Hyper.Web.Areas.Admin.Models.Appointments;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class AppointmentController : BaseAdminController
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentFactory _appointmentFactory;
        private readonly IProductService _productService;
        private readonly IPatientService _patientService;
        private readonly IEmployeeService _employeeService;
        private readonly IStoreContext _storeContext;
        private readonly ISmsSender _smsSender;
        public AppointmentController(IAppointmentService appointmentService,
            IAppointmentFactory appointmentFactory,
            IProductService productService,
            IStoreContext storeContext,
            ISmsSender smsSender,
            IPatientService patientService,
            IEmployeeService employeeService)
        {
            _appointmentService = appointmentService;
            _appointmentFactory = appointmentFactory;
            _productService = productService;
            _storeContext = storeContext;
            _smsSender = smsSender;
            _patientService = patientService;
            _employeeService = employeeService;
        }

        public virtual IActionResult Index()
        {
            var model = new AppointmentSearchModel()
            {
                SaturdayHoliday = _storeContext.CurrentStore.SaturdayHoliday,
                SundayHoliday = _storeContext.CurrentStore.SundayHoliday,
                BusinessStartTime = $"\"{_storeContext.CurrentStore.BusinessStartTime}\"",
                BusinessEndTime = $"\"{_storeContext.CurrentStore.BusinessEndTime}\""
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApoointmentCalendar(AppointmentCalenderItemSearchModel model)
        {
            var appointments = _appointmentService.SearchAppointments(
                patientName: model.PatientName,
                patientSurname: model.PatientSurname,
                startDateTime: model.start,
                endDateTime: model.end
                );

            appointments.ForEach(x =>
            {
                x.Color = _appointmentFactory.PrepareColor(x.PaymentTypeId, x.PaymentStatusId, x.AppointmentStatusId);
            });

            return Json(appointments);
        }

        [ResponseCache(Duration = 300)]
        public virtual IActionResult Create()
        {
            var model = _appointmentFactory.PrepareAppointmentCreateModel(new AppointmentCreateModel());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Create(AppointmentCreateModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return Json(new ErrorResult());

            var startDate = model.StartDate.ToTurkishDateTime();
            var startTime = model.StartTime.ToTurkishDateTime();
            var startDateTime = startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
            var product = _productService.GetById(model.ProductId);

            var firstAppointment = _appointmentFactory.PrepareAppointment(model, product, startDateTime);
            _appointmentService.Insert(firstAppointment);

            if (model.SendSmsToPatient)
            {
                var patient = _patientService.GetById(model.AssignedPatientId);
                var employee = _employeeService.GetById(model.AssignedEmployeeId);
                if (patient.SendSmsNotifications)
                {
                    var messageBody = $"Sayın {patient.FullName.ToUpper()}, {employee.FullName.ToUpper()} için {startDateTime:dd.MM.yyyy HH:mm} tarihli randevunuz oluşturulmuştur.";
                    _smsSender.SendSmsAsync(messageBody, patient.Phone);
                }
                if (employee.SendSmsNotifications)
                {
                    var messageBody = $"Sayın {employee.FullName.ToUpper()}, {patient.FullName.ToUpper()} için {startDateTime:dd.MM.yyyy HH:mm} tarihli randevunuz oluşturulmuştur.";
                    _smsSender.SendSmsAsync(messageBody, employee.Phone);
                }
            }

            //List<Appointment> appointments = new();
            //var period = (PeriodType)model.PeriodTypeId;
            //for (int i = 0; i < model.PeriodCount; i++)
            //{
            //    switch (period)
            //    {
            //        case PeriodType.Daily:
            //            startDateTime = startDateTime.AddDays(1);
            //            appointments.Add(_appointmentFactory.PrepareAppointment(model, product, startDateTime));

            //            break;
            //        case PeriodType.Weekly:
            //            startDateTime = startDateTime.AddDays(7);
            //            appointments.Add(_appointmentFactory.PrepareAppointment(model, product, startDateTime));

            //            break;
            //        case PeriodType.PerTwoWeek:
            //            startDateTime = startDateTime.AddDays(14);
            //            appointments.Add(_appointmentFactory.PrepareAppointment(model, product, startDateTime));

            //            break;
            //        case PeriodType.PerThreeWeek:
            //            startDateTime = startDateTime.AddDays(21);
            //            appointments.Add(_appointmentFactory.PrepareAppointment(model, product, startDateTime));

            //            break;
            //        case PeriodType.Monthly:
            //            startDateTime = startDateTime.AddMonths(1);
            //            appointments.Add(_appointmentFactory.PrepareAppointment(model, product, startDateTime));
            //            break;
            //        default:
            //            break;
            //    }

            //}
            //_appointmentService.InsertRange(appointments);

            return Json(new SuccessResult());
        }


        public virtual IActionResult Edit(int id)
        {
            var entity = _appointmentService.GetById(id);
            if (entity is null)
                return Content("");

            var model = _appointmentFactory.PrepareAppointmentEditModel(entity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Edit(AppointmentEditModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return Json(new ErrorResult());

            var product = _productService.GetById(model.ProductId);
            if (product is null)
                return Json(new ErrorResult());

            var appointment = _appointmentService.GetById(model.Id);
            if (appointment is null)
                return Json(new ErrorResult());

            var startDate = model.StartDate.ToTurkishDateTime();
            var startTime = model.StartTime.ToTurkishDateTime();
            var startDateTime = startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);

            appointment = _appointmentFactory.PrepareAppointment(model, appointment, product, startDateTime);
            _appointmentService.Update(appointment);
            return Json(new SuccessResult());
        }
    }
}