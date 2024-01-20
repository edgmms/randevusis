using Hyper.Core;
using Hyper.Core.Domain.Appointments;
using Hyper.Services.Appointments;
using Hyper.Services.Employees;
using Hyper.Services.Patients;
using Hyper.Services.Products;
using Hyper.Web.Areas.Admin.Models.Reports;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyper.Web.Areas.Admin.Factories.Reports
{
    public class ReportFactory : IReportFactory
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmployeeService _employeeService;
        private readonly IPatientService _patientService;
        private readonly IProductService _productService;

        public ReportFactory(IAppointmentService appointmentService,
            IEmployeeService employeeService,
            IPatientService patientService,
            IProductService productService)
        {
            _appointmentService = appointmentService;
            _employeeService = employeeService;
            _patientService = patientService;
            _productService = productService;
        }

        public CashReportModel PrepareCacheReportModel()
        {
            var now = DateTime.Now;
            var baseDate = DateTime.Today;
            var today = baseDate;
            //var yesterday = baseDate.AddDays(-1);
            var nextDay = baseDate.AddDays(+1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            //var lastWeekStart = thisWeekStart.AddDays(-7);
            //var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            //var lastMonthStart = thisMonthStart.AddMonths(-1);
            //var lastMonthEnd = thisMonthStart.AddSeconds(-1);

            var appointments = _appointmentService.SearchAppointments(startDateTimeFrom: thisMonthStart, startDateTimeTo: thisMonthEnd);

            var employeeIds = appointments.Select(x => x.AssignedEmployeeId).ToArray();
            var employees = _employeeService.GetByIds(employeeIds);

            var patientIds = appointments.Select(x => x.AssignedPatientId).ToArray();
            var patients = _patientService.GetByIds(patientIds);

            var productIds = appointments.Select(x => x.ProductId).ToArray();
            var products = _productService.GetByIds(productIds);


            var model = new CashReportModel
            {
                DailyHeaderText = $"{today.ToShortDateString()} 00:00 - {today.ToShortDateString()} 23:59 arasındaki randevulara göre hazırlanmıştır.",
                WeeklyHeaderText = $"{thisWeekStart.ToShortDateString()} 00:00 - {thisWeekEnd.ToShortDateString()} 23:59 arasındaki randevulara göre hazırlanmıştır.",
                MonthlyHeaderText = $"{thisMonthStart.ToShortDateString()} 00:00 - {thisMonthEnd.ToShortDateString()} 23:59 arasındaki randevulara göre hazırlanmıştır.",

                //daily counts
                DailyTotalAppointmentPendingCount = appointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Pending),
                DailyTotalAppointmentCanceledCount = appointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled),
                DailyTotalAppointmentPostponedCount = appointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Postponed),
                DailyTotalAppointmentPatientConfirmedCount = appointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.PatientConfirmed),
                DailyTotalAppointmentCompletedCount = appointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed),

                //weekly counts
                WeeklyTotalAppointmentPendingCount = appointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Pending),
                WeeklyTotalAppointmentCanceledCount = appointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled),
                WeeklyTotalAppointmentPostponedCount = appointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Postponed),
                WeeklyTotalAppointmentPatientConfirmedCount = appointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed),
                WeeklyTotalAppointmentCompletedCount = appointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed),

                //monthly counts
                MonthlyTotalAppointmentPendingCount = appointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Pending),
                MonthlyTotalAppointmentCanceledCount = appointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled),
                MonthlyTotalAppointmentPostponedCount = appointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Postponed),
                MonthlyTotalAppointmentPatientConfirmedCount = appointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed),
                MonthlyTotalAppointmentCompletedCount = appointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed),


                /*------------------------------*/
                //daily totals

                DailyAppointmentPendingTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.ProductPrice),
                DailyAppointmentPendingTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.DiscountedPrice),

                DailyAppointmentPostponedTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.ProductPrice),
                DailyAppointmentPostponedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.DiscountedPrice),

                DailyAppointmentPatientConfirmedTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.ProductPrice),
                DailyAppointmentPatientConfirmedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.DiscountedPrice),

                DailyAppointmentCanceledTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.ProductPrice),
                DailyAppointmentCanceledTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),

                DailyAppointmentCompletedTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.ProductPrice),
                DailyAppointmentCompletedCreditCardTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.ProductPrice),
                DailyAppointmentCompletedCashTotalPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.ProductPrice),

                DailyAppointmentCompletedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.DiscountedPrice),
                DailyAppointmentCompletedCreditCardTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.DiscountedPrice),
                DailyAppointmentCompletedCashTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.DiscountedPrice),


                /*------------------------------*/
                //weekly totals

                WeeklyAppointmentPendingTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.ProductPrice),
                WeeklyAppointmentPendingTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.DiscountedPrice),

                WeeklyAppointmentPostponedTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.ProductPrice),
                WeeklyAppointmentPostponedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.DiscountedPrice),
                    
                WeeklyAppointmentPatientConfirmedTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.ProductPrice),
                WeeklyAppointmentPatientConfirmedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.DiscountedPrice),

                WeeklyAppointmentCanceledTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.ProductPrice),
                WeeklyAppointmentCanceledTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),

                WeeklyAppointmentCompletedTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.ProductPrice),
                WeeklyAppointmentCompletedCreditCardTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.ProductPrice),
                WeeklyAppointmentCompletedCashTotalPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.ProductPrice),

                WeeklyAppointmentCompletedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.DiscountedPrice),
                WeeklyAppointmentCompletedCreditCardTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.DiscountedPrice),
                WeeklyAppointmentCompletedCashTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.DiscountedPrice),

                /*------------------------------*/
                //monthly totals

                MonthlyAppointmentPendingTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.ProductPrice),
                MonthlyAppointmentPendingTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Pending).Sum(x => x.DiscountedPrice),

                MonthlyAppointmentPostponedTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.ProductPrice),
                MonthlyAppointmentPostponedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Postponed).Sum(x => x.DiscountedPrice),
                 
                MonthlyAppointmentPatientConfirmedTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.ProductPrice),
                MonthlyAppointmentPatientConfirmedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.PatientConfirmed).Sum(x => x.DiscountedPrice),

                MonthlyAppointmentCanceledTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.ProductPrice),
                MonthlyAppointmentCanceledTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),

                MonthlyAppointmentCompletedTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.ProductPrice),
                MonthlyAppointmentCompletedCreditCardTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.ProductPrice),
                MonthlyAppointmentCompletedCashTotalPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.ProductPrice),

                MonthlyAppointmentCompletedTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed).Sum(x => x.DiscountedPrice),
                MonthlyAppointmentCompletedCreditCardTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.CreditCard).Sum(x => x.DiscountedPrice),
                MonthlyAppointmentCompletedCashTotalDiscountedPrice = appointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed && x.PaymentStatus == PaymentStatus.Completed && x.PaymentType == PaymentType.Cash).Sum(x => x.DiscountedPrice),
            };

            foreach (var employee in employees)
            {
                var employeeAppointments = appointments.Where(x => x.AssignedEmployeeId == employee.Id);

                model.EmployeeCashReport.Add(new CashReportModel.EmployeeReportModel
                {
                    EmployeeId = employee.Id,
                    FullName = employee.FullName,
                    //daily counts
                    DailyAppointmentCompletedCount = employeeAppointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed),
                    DailyAppointmentCanceledCount = employeeAppointments.Count(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled),
                    //weekly counts
                    WeeklyAppointmentCompletedCount = employeeAppointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed),
                    WeeklyAppointmentCanceledCount = employeeAppointments.Count(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled),
                    //monthly counts
                    MonthlyAppointmentCompletedCount = employeeAppointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed),
                    MonthlyAppointmentCanceledCount = employeeAppointments.Count(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled),
                    //daily totals
                    DailyAppointmentCompletedTotalPrice = employeeAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.ProductPrice),
                    DailyAppointmentCompletedTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.ProductPrice),
                    DailyAppointmentCanceledTotalPrice = employeeAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),
                    DailyAppointmentCanceledTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),
                    //weekly totals
                    WeeklyAppointmentCompletedTotalPrice = employeeAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.ProductPrice),
                    WeeklyAppointmentCompletedTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.DiscountedPrice),
                    WeeklyAppointmentCanceledTotalPrice = employeeAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.ProductPrice),
                    WeeklyAppointmentCanceledTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),
                    //monthly totals
                    MonthlyAppointmentCompletedTotalPrice = employeeAppointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.ProductPrice),
                    MonthlyAppointmentCompletedTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Completed).Sum(x => x.DiscountedPrice),
                    MonthlyAppointmentCanceledTotalPrice = employeeAppointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.ProductPrice),
                    MonthlyAppointmentCanceledTotalDiscountedPrice = employeeAppointments.Where(x => x.StartDateTime > thisMonthStart && x.StartDateTime < thisMonthEnd && x.AppointmentStatus == AppointmentStatus.Canceled).Sum(x => x.DiscountedPrice),
                });
            }

            foreach (var product in products)
            {
                var productAppointments = appointments.Where(x => x.ProductId == product.Id);
                model.ProductCashReport.Add(new CashReportModel.ProductReportModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    DailyProductTotalPrice = productAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay).Sum(x => x.ProductPrice),
                    DailyProductTotalDiscountedPrice = productAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay).Sum(x => x.DiscountedPrice),
                    DailyProductTotalPriceTaxExcluded = productAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay).Sum(x => x.ProductPrice) / (1 + (product.TaxRate / 100)),
                    DailyProductTotalDiscountedPriceTaxExcluded = productAppointments.Where(x => x.StartDateTime > today && x.StartDateTime < nextDay).Sum(x => x.DiscountedPrice) / (1 + (product.TaxRate / 100)),
                    WeeklyProductTotalPrice = productAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd).Sum(x => x.ProductPrice),
                    WeeklyProductTotalDiscountedPrice = productAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd).Sum(x => x.DiscountedPrice),
                    WeeklyProductTotalPriceTaxExcluded = productAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd).Sum(x => x.ProductPrice) / (1 + (product.TaxRate / 100)),
                    WeeklyProductTotalDiscountedPriceTaxExcluded = productAppointments.Where(x => x.StartDateTime > thisWeekStart && x.StartDateTime < thisWeekEnd).Sum(x => x.DiscountedPrice) / (1 + (product.TaxRate / 100)),
                    MonthlyProductTotalPrice = productAppointments.Sum(x => x.ProductPrice),
                    MonthlyProductTotalDiscountedPrice = productAppointments.Sum(x => x.DiscountedPrice),
                    MonthlyProductTotalPriceTaxExcluded = productAppointments.Sum(x => x.ProductPrice) / (1 + (product.TaxRate / 100)),
                    MonthlyProductTotalDiscountedPriceTaxExcluded = productAppointments.Sum(x => x.DiscountedPrice) / (1 + (product.TaxRate / 100)),
                });
            }

            return model;
        }
    }
}