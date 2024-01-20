using Hyper.Core.Domain.Organisations;
using Hyper.Web.Areas.Admin.Models.Stores;
using System;

namespace Hyper.Web.Areas.Admin.Factories.Stores
{
	public class StoreFactory : IStoreFactory
	{
	
		public StoreModel PrepareStoreModel(Store store, StoreModel model = null)
		{
			if (store is null)
				throw new ArgumentNullException(nameof(store));

			if (model is null)
				model = new StoreModel();

			model.Id = store.Id;
			model.Address = store.Address;
			model.Email = store.Email;
			model.Name = store.Name;
			model.PhoneNumber = store.PhoneNumber;
			model.TaxAdministration = store.TaxAdministration;
			model.TaxNumber = store.TaxNumber;
			model.BusinessStartTime = store.BusinessStartTime;
			model.BusinessEndTime = store.BusinessEndTime;
			model.SaturdayHoliday = store.SaturdayHoliday;
			model.SundayHoliday = store.SundayHoliday;
			model.AppointmentStatusPendingColor = store.AppointmentStatusPendingColor;
			model.AppointmentStatusCanceledColor = store.AppointmentStatusCanceledColor;
			model.AppointmentStatusPostponedColor = store.AppointmentStatusPostponedColor;
			model.AppointmentStatusPatientConfirmedColor = store.AppointmentStatusPatientConfirmedColor;
			model.AppointmentStatusCompletedColor = store.AppointmentStatusCompletedColor;
			model.PaymentStatusPendingColor = store.PaymentStatusPendingColor;
			model.PaymentStatusCompletedColor = store.PaymentStatusCompletedColor;
			model.PaymentTypeCashColor = store.PaymentTypeCashColor;
			model.PaymentTypeCreditCardColor = store.PaymentTypeCreditCardColor;
			model.SmsUsername = store.SmsUsername;
			model.SmsPassword = store.SmsPassword;
			model.SmsHeader= store.SmsHeader;

			return model;
		}

		public Store PrepareStore(Store store, StoreModel model)
		{
			if (store is null)
				throw new ArgumentNullException(nameof(store));

			if (model is null)
				throw new ArgumentNullException(nameof(model));

			store.Address = model.Address;
			store.Email = model.Email;
			store.Name = model.Name;
			store.PhoneNumber = model.PhoneNumber;
			store.TaxAdministration = model.TaxAdministration;
			store.TaxNumber = model.TaxNumber;
			store.BusinessStartTime = model.BusinessStartTime;
			store.BusinessEndTime = model.BusinessEndTime;
			store.SaturdayHoliday = model.SaturdayHoliday;
			store.SundayHoliday = model.SundayHoliday;
			store.AppointmentStatusPendingColor = model.AppointmentStatusPendingColor;
			store.AppointmentStatusCanceledColor = model.AppointmentStatusCanceledColor;
			store.AppointmentStatusPostponedColor = model.AppointmentStatusPostponedColor;
			store.AppointmentStatusPatientConfirmedColor = model.AppointmentStatusPatientConfirmedColor;
			store.AppointmentStatusCompletedColor = model.AppointmentStatusCompletedColor;
			store.PaymentStatusPendingColor = model.PaymentStatusPendingColor;
			store.PaymentStatusCompletedColor = model.PaymentStatusCompletedColor;
			store.PaymentTypeCashColor = model.PaymentTypeCashColor;
			store.PaymentTypeCreditCardColor = model.PaymentTypeCreditCardColor;
            store.SmsUsername = model.SmsUsername;
            store.SmsPassword = model.SmsPassword;
            store.SmsHeader = model.SmsHeader;

            return store;
		}

	}
}
