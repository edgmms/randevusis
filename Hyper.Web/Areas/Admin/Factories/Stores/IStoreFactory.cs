using Hyper.Core.Domain.Organisations;
using Hyper.Web.Areas.Admin.Models.Stores;

namespace Hyper.Web.Areas.Admin.Factories.Stores
{
	public interface IStoreFactory
	{
		StoreModel PrepareStoreModel(Store store, StoreModel model = null);

		Store PrepareStore(Store store, StoreModel model);
	}
}
