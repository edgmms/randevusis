using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using Hyper.Core.Domain.Products;
using Hyper.Core.Extensions;
using Hyper.Services.Employees;
using Hyper.Services.Patients;
using Hyper.Services.Products;
using Hyper.Web.Areas.Admin.Models.Appointments;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyper.Web.Areas.Admin.Factories.Appointments
{
    public class AppointmentFactory : IAppointmentFactory
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPatientService _patientService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public AppointmentFactory(IEmployeeService employeeService,
            IPatientService patientService,
            IProductService productService,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _employeeService = employeeService;
            _patientService = patientService;
            _productService = productService;
            _workContext = workContext;
            _storeContext = storeContext;
        }

        public Appointment PrepareAppointment(AppointmentCreateModel model, Product product, DateTime startDateTime)
        {
            var appointment = new Appointment
            {
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddMinutes(model.DurationAsMinute),
                AssignedPatientId = model.AssignedPatientId,
                AssignedEmployeeId = model.AssignedEmployeeId,
                AppointmentStatus = AppointmentStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                PaymentType = PaymentType.Cash,
                Active = true,
                ProductId = model.ProductId,
                ProductPrice = product.Price,
                DiscountedPrice = product.Price - model.DiscountTotal,
                DiscountTotal = model.DiscountTotal,
                AppointmentCost = (product.Price - model.DiscountTotal) - product.ProductCost,
                TaxRate = product.TaxRate,
                Note = model.Note
            };

            return appointment;
        }

        public Appointment PrepareAppointment(AppointmentEditModel model, Appointment entity, Product product, DateTime startDateTime)
        {
            entity.StartDateTime = startDateTime;
            entity.EndDateTime = startDateTime.AddMinutes(model.DurationAsMinute);

            if (_workContext.IsStoreAdministrator)
                entity.AssignedEmployeeId = model.AssignedEmployeeId;

            entity.AppointmentStatusId = model.AppointmentStatusId;
            entity.PaymentStatusId = model.PaymentStatusId;
            entity.PaymentTypeId = model.PaymentTypeId;
            entity.Active = true;
            entity.ProductId = model.ProductId;
            entity.ProductPrice = product.Price;
            entity.DiscountedPrice = product.Price - model.DiscountTotal;
            entity.DiscountTotal = model.DiscountTotal;
            entity.AppointmentCost = (product.Price - model.DiscountTotal) - product.ProductCost;
            entity.TaxRate = product.TaxRate;
            entity.Note = model.Note;

            return entity;
        }

        public AppointmentCreateModel PrepareAppointmentCreateModel(AppointmentCreateModel model)
        {
            model.DurationAsMinute = 15;
            model.AvailableDurations.Add(new SelectListItem { Text = "10 Dakika", Value = "10", });
            model.AvailableDurations.Add(new SelectListItem { Text = "15 Dakika", Value = "15", Selected = true });
            model.AvailableDurations.Add(new SelectListItem { Text = "20 Dakika", Value = "20", });
            model.AvailableDurations.Add(new SelectListItem { Text = "25 Dakika", Value = "25", });
            model.AvailableDurations.Add(new SelectListItem { Text = "30 Dakika", Value = "30", });
            model.AvailableDurations.Add(new SelectListItem { Text = "45 Dakika", Value = "45", });
            model.AvailableDurations.Add(new SelectListItem { Text = "60 Dakika", Value = "60", });
            model.AvailableDurations.Add(new SelectListItem { Text = "120 Dakika", Value = "120", });

            //employees
            model.AvailableEmployees = _employeeService.SearchEmployees().Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Id.ToString(),
            }).ToList();

            if (!model.AvailableEmployees.Any())
            {
                model.AvailableEmployees.Insert(0, new SelectListItem { Text = "Lütfen personel ekleyiniz", Value = "0", Disabled = true });
            }

            //patients
            if (model.AssignedPatientId > 0)
            {
                var patient = _patientService.GetById(model.AssignedPatientId);
                model.AvailablePatients.Add(new SelectListItem { Text = patient.FullName, Value = "0", Selected = true });
            }
            model.AvailablePatients.Insert(0, new SelectListItem { Text = "Seçiniz", Value = "0", Disabled = true });

            //products
            model.AvailableProducts = _productService.SearchProducts()
                .Select(x => new SelectListItem
                {
                    Text = $"{(x.ProductType == ProductType.Product ? "Ü" : "H")} » {x.Name} » {x.Price}",
                    Value = x.Id.ToString(),
                    Selected = model.ProductId == x.Id
                })
                .OrderBy(x => x.Value)
                .ToList();

            if (!model.AvailableProducts.Any())
            {
                model.AvailableProducts.Insert(0, new SelectListItem { Text = "Lütfen ürün veya hizmet ekleyiniz", Value = "0", Disabled = true, Selected = true });
            }

            //types and status
            model.AvailablePeriodTypes = PeriodType.OneTime.ToSelectList(markCurrentAsSelected: false);
            model.PeriodCount = 1;
            return model;
        }

        public AppointmentEditModel PrepareAppointmentEditModel(Appointment entity)
        {
            var patient = _patientService.GetById(entity.AssignedPatientId);
            var products = _productService.SearchProducts();
            var employees = _employeeService.SearchEmployees();

            var model = new AppointmentEditModel
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                AssignedEmployeeId = entity.AssignedEmployeeId,
                AppointmentStatusId = entity.AppointmentStatusId,
                PaymentStatusId = entity.PaymentStatusId,
                PaymentTypeId = entity.PaymentTypeId,
                DiscountTotal = entity.DiscountTotal,
                TotalPrice = entity.DiscountedPrice,
                StartDate = entity.StartDateTime.ToTurkishDate(),
                StartTime = entity.StartDateTime.ToTurkishHour(),
                DurationAsMinute = Convert.ToInt32((entity.EndDateTime - entity.StartDateTime).TotalMinutes),
                IsStoreAdministrator = _workContext.IsStoreAdministrator,
                AssingedPatientText = $"{patient.FullName}, {patient.Phone}, {patient.TurkishIdentityNumber}, {patient.Address}",
                AssingedPatientTurkishIdentityNumber = patient.TurkishIdentityNumber,
                Color = this.PrepareColor(entity.PaymentTypeId, entity.PaymentStatusId, entity.AppointmentStatusId),
                Note = entity.Note,
            };

            var durations = new List<SelectListItem> {
                new SelectListItem { Text = "10 Dakika", Value = "10", Selected = model.DurationAsMinute == 10 },
                new SelectListItem { Text = "15 Dakika", Value = "15", Selected = model.DurationAsMinute == 15 },
                new SelectListItem { Text = "20 Dakika", Value = "20", Selected = model.DurationAsMinute == 20 },
                new SelectListItem { Text = "25 Dakika", Value = "25", Selected = model.DurationAsMinute == 25 },
                new SelectListItem { Text = "30 Dakika", Value = "30", Selected = model.DurationAsMinute == 30 },
                new SelectListItem { Text = "45 Dakika", Value = "45", Selected = model.DurationAsMinute == 45 },
                new SelectListItem { Text = "60 Dakika", Value = "60", Selected = model.DurationAsMinute == 60 },
                new SelectListItem { Text = "120 Dakika", Value = "120", Selected = model.DurationAsMinute == 120 }
            };

            model.AvailableDurations.AddRange(durations);

            //employees
            model.AvailableEmployees = employees.Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Id.ToString(),
                Selected = model.AssignedEmployeeId == x.Id
            }).ToList();
            model.AvailableEmployees.Insert(0, new SelectListItem { Text = "Seçiniz", Value = "0", Disabled = true });

            //products
            if (products.Any())
            {
                model.AvailableProducts = products
                   .Select(x => new SelectListItem
                   {
                       Text = $"{(x.ProductType == ProductType.Product ? "Ü" : "H")} » {x.Name} » {x.Price}",
                       Value = x.Id.ToString(),
                       Selected = model.ProductId == x.Id
                   })
                   .OrderBy(x => x.Text)
                   .ToList();
            }
            else
            {
                model.AvailableProducts.Insert(0, new SelectListItem { Text = "Lütfen ürün veya hizmet ekleyiniz", Value = "0", Disabled = true, Selected = true });
            }

            //types and status
            model.AvailablePaymentTypes = ((PaymentType)entity.PaymentTypeId).ToSelectList();
            model.AvailablePaymentStatuses = ((PaymentStatus)entity.PaymentStatusId).ToSelectList();
            model.AvailableAppointmentStatuses = ((AppointmentStatus)entity.AppointmentStatusId).ToSelectList();

            return model;
        }

        public string PrepareColor(int paymentTypeId, int paymentStatusId, int appointmentStatusId)
        {
            var paymentType = (PaymentType)paymentTypeId;
            var paymentStatus = (PaymentStatus)paymentStatusId;
            var appointmentStatus = (AppointmentStatus)appointmentStatusId;

            switch (appointmentStatus)
            {
                case AppointmentStatus.Canceled:
                    return _storeContext.CurrentStore.AppointmentStatusCanceledColor;
                case AppointmentStatus.Completed:
                    if (paymentStatus == PaymentStatus.Pending)
                    {
                        return _storeContext.CurrentStore.PaymentStatusPendingColor;
                    }
                    else
                    {
                        switch (paymentType)
                        {
                            case PaymentType.Cash:
                                return _storeContext.CurrentStore.PaymentTypeCashColor;
                            case PaymentType.CreditCard:
                            default:
                                return _storeContext.CurrentStore.PaymentTypeCreditCardColor;
                        }
                    }
                case AppointmentStatus.Postponed:
                    return _storeContext.CurrentStore.AppointmentStatusPostponedColor;
                case AppointmentStatus.PatientConfirmed:
                    return _storeContext.CurrentStore.AppointmentStatusPatientConfirmedColor;
                case AppointmentStatus.Pending:
                default:
                    return _storeContext.CurrentStore.AppointmentStatusPendingColor;
            }
        }
    }
}