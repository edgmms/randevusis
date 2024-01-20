using System;

namespace Hyper.Core.Domain.Appointments
{
    public class Appointment : FullAuditedEntity<int>, IStoreEntity
    {
        public int RegisteredInStoreId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int AssignedEmployeeId { get; set; }

        public int AssignedPatientId { get; set; }

        public decimal DiscountTotal { get; set; }

        public decimal DiscountedPrice { get; set; }

        public decimal AppointmentCost { get; set; }

        public int ProductId { get; set; }

        public decimal ProductPrice { get; set; }

        public int TaxRate { get; set; }

        public string Note { get; set; }

        public int PaymentTypeId { get; set; }

        public int PaymentStatusId { get; set; }

        public int AppointmentStatusId { get; set; }

        public PaymentType PaymentType
        {
            get
            {
                return (PaymentType)PaymentTypeId;
            }
            set
            {
                PaymentTypeId = (int)value;
            }
        }

        public PaymentStatus PaymentStatus
        {
            get
            {
                return (PaymentStatus)PaymentStatusId;
            }
            set
            {
                PaymentStatusId = (int)value;
            }
        }

        public AppointmentStatus AppointmentStatus
        {
            get
            {
                return (AppointmentStatus)AppointmentStatusId;
            }
            set
            {
                AppointmentStatusId = (int)value;
            }
        }

    }
}
