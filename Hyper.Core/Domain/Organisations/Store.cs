using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Hyper.Core.Domain.Organisations
{
	public class Store : BaseEntity<int>, ISoftDeletedEntity
	{
		public int ParentStoreId { get; set; }
		public string Name { get; set; }
		public string TaxAdministration { get; set; }
		public string TaxNumber { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public string BusinessStartTime { get; set; }
		public string BusinessEndTime { get; set; }
		public bool SaturdayHoliday { get; set; }
		public bool SundayHoliday { get; set; }
		public string AppointmentStatusPendingColor { get; set; }
		public string AppointmentStatusCanceledColor { get; set; }
		public string AppointmentStatusPostponedColor { get; set; }
		public string AppointmentStatusPatientConfirmedColor { get; set; }
		public string AppointmentStatusCompletedColor { get; set; }
		public string PaymentStatusPendingColor { get; set; }
		public string PaymentStatusCompletedColor { get; set; }
		public string PaymentTypeCashColor { get; set; }
		public string PaymentTypeCreditCardColor { get; set; }

		public string SmsUsername { get; set; }
		public string SmsPassword { get; set; }
		public string SmsHeader { get; set; }

		public bool Deleted { get; set; }
	}
}
