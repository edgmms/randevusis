using System.Collections.Generic;

namespace Hyper.Web.Areas.Admin.Models.Reports
{
    public class CashReportModel
    {
        public CashReportModel()
        {
            this.EmployeeCashReport = new List<EmployeeReportModel>();
            this.ProductCashReport = new List<ProductReportModel>();
        }
        public string DailyHeaderText { get; set; }
        public string WeeklyHeaderText { get; set; }
        public string MonthlyHeaderText { get; set; }

        public int DailyTotalAppointmentCompletedCount { get; set; }
        public int DailyTotalAppointmentPostponedCount { get; set; }
        public int DailyTotalAppointmentPatientConfirmedCount { get; set; }
        public int DailyTotalAppointmentPendingCount { get; set; }
        public int DailyTotalAppointmentCanceledCount { get; set; }
        public decimal DailyAppointmentCompletedTotalPrice { get; set; }
        public decimal DailyAppointmentCompletedCreditCardTotalPrice { get; set; }
        public decimal DailyAppointmentCompletedCashTotalPrice { get; set; }
        public decimal DailyAppointmentCompletedTotalDiscountedPrice { get; set; }
        public decimal DailyAppointmentCompletedCreditCardTotalDiscountedPrice { get; set; }
        public decimal DailyAppointmentCompletedCashTotalDiscountedPrice { get; set; }
        public decimal DailyAppointmentCanceledTotalPrice { get; set; }
        public decimal DailyAppointmentCanceledTotalDiscountedPrice { get; set; }
        public decimal DailyAppointmentPendingTotalPrice { get; set; }
        public decimal DailyAppointmentPendingTotalDiscountedPrice { get; set; }
        public decimal DailyAppointmentPostponedTotalPrice { get; set; }
        public decimal DailyAppointmentPostponedTotalDiscountedPrice { get; set; }     
        public decimal DailyAppointmentPatientConfirmedTotalPrice { get; set; }
        public decimal DailyAppointmentPatientConfirmedTotalDiscountedPrice { get; set; }

        public int WeeklyTotalAppointmentCompletedCount { get; set; }
        public int WeeklyTotalAppointmentPostponedCount { get; set; }
        public int WeeklyTotalAppointmentPendingCount { get; set; }
        public int WeeklyTotalAppointmentCanceledCount { get; set; }
        public int WeeklyTotalAppointmentPatientConfirmedCount { get; set; }
        public decimal WeeklyAppointmentCompletedTotalPrice { get; set; }
        public decimal WeeklyAppointmentCompletedCreditCardTotalPrice { get; set; }
        public decimal WeeklyAppointmentCompletedCashTotalPrice { get; set; }
        public decimal WeeklyAppointmentCompletedTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentCompletedCreditCardTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentCompletedCashTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentCanceledTotalPrice { get; set; }
        public decimal WeeklyAppointmentCanceledTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentPendingTotalPrice { get; set; }
        public decimal WeeklyAppointmentPendingTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentPostponedTotalPrice { get; set; }
        public decimal WeeklyAppointmentPostponedTotalDiscountedPrice { get; set; }
        public decimal WeeklyAppointmentPatientConfirmedTotalPrice { get; set; }
        public decimal WeeklyAppointmentPatientConfirmedTotalDiscountedPrice { get; set; }


        public int MonthlyTotalAppointmentCompletedCount { get; set; }
        public int MonthlyTotalAppointmentPostponedCount { get; set; }
        public int MonthlyTotalAppointmentPendingCount { get; set; }
        public int MonthlyTotalAppointmentPatientConfirmedCount { get; set; }
        public int MonthlyTotalAppointmentCanceledCount { get; set; }
        public decimal MonthlyAppointmentCompletedTotalPrice { get; set; }
        public decimal MonthlyAppointmentCompletedCreditCardTotalPrice { get; set; }
        public decimal MonthlyAppointmentCompletedCashTotalPrice { get; set; }
        public decimal MonthlyAppointmentCompletedTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentCompletedCreditCardTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentCompletedCashTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentCanceledTotalPrice { get; set; }
        public decimal MonthlyAppointmentCanceledTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentPendingTotalPrice { get; set; }
        public decimal MonthlyAppointmentPendingTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentPostponedTotalPrice { get; set; }
        public decimal MonthlyAppointmentPostponedTotalDiscountedPrice { get; set; }
        public decimal MonthlyAppointmentPatientConfirmedTotalPrice { get; set; }
        public decimal MonthlyAppointmentPatientConfirmedTotalDiscountedPrice { get; set; }




        public List<EmployeeReportModel> EmployeeCashReport { get; set; }
        public List<ProductReportModel> ProductCashReport { get; set; }
        public class EmployeeReportModel
        {
            public int EmployeeId { get; set; }
            public string FullName { get; set; }

            public int DailyAppointmentCompletedCount { get; set; }
            public int DailyAppointmentCanceledCount { get; set; }

            public int WeeklyAppointmentCompletedCount { get; set; }
            public int WeeklyAppointmentCanceledCount { get; set; }

            public int MonthlyAppointmentCompletedCount { get; set; }
            public int MonthlyAppointmentCanceledCount { get; set; }

            public decimal DailyAppointmentCompletedTotalPrice { get; set; }
            public decimal DailyAppointmentCompletedTotalDiscountedPrice { get; set; }
            public decimal DailyAppointmentCanceledTotalPrice { get; set; }
            public decimal DailyAppointmentCanceledTotalDiscountedPrice { get; set; }

            public decimal WeeklyAppointmentCompletedTotalPrice { get; set; }
            public decimal WeeklyAppointmentCompletedTotalDiscountedPrice { get; set; }
            public decimal WeeklyAppointmentCanceledTotalPrice { get; set; }
            public decimal WeeklyAppointmentCanceledTotalDiscountedPrice { get; set; }

            public decimal MonthlyAppointmentCompletedTotalPrice { get; set; }
            public decimal MonthlyAppointmentCompletedTotalDiscountedPrice { get; set; }
            public decimal MonthlyAppointmentCanceledTotalPrice { get; set; }
            public decimal MonthlyAppointmentCanceledTotalDiscountedPrice { get; set; }
        }
        public class ProductReportModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }

            public decimal DailyProductTotalPrice { get; set; }
            public decimal DailyProductTotalPriceTaxExcluded { get; set; }
            public decimal DailyProductTotalDiscountedPrice { get; set; }
            public decimal DailyProductTotalDiscountedPriceTaxExcluded { get; set; }

            public decimal WeeklyProductTotalPrice { get; set; }
            public decimal WeeklyProductTotalPriceTaxExcluded { get; set; }
            public decimal WeeklyProductTotalDiscountedPrice { get; set; }
            public decimal WeeklyProductTotalDiscountedPriceTaxExcluded { get; set; }

            public decimal MonthlyProductTotalPrice { get; set; }
            public decimal MonthlyProductTotalPriceTaxExcluded { get; set; }
            public decimal MonthlyProductTotalDiscountedPrice { get; set; }
            public decimal MonthlyProductTotalDiscountedPriceTaxExcluded { get; set; }

        }
    }
}