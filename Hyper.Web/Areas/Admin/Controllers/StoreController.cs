using Hyper.Core;
using Hyper.Services.Organisations;
using Hyper.Web.Areas.Admin.Factories.Stores;
using Hyper.Web.Areas.Admin.Models.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
	public class StoreController : BaseAdminController
	{
		private readonly IStoreService _storeService;
		private readonly IStoreContext _storeContext;
		private readonly IStoreFactory _storeFactory;

		public StoreController(IStoreService storeService, IStoreContext storeContext, IStoreFactory storeFactory)
		{
			_storeService = storeService;
			_storeContext = storeContext;
			_storeFactory = storeFactory;
		}

		public IActionResult Edit()
		{
			var store = _storeContext.CurrentStore;
			var model = _storeFactory.PrepareStoreModel(store, new StoreModel());
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(StoreModel model)
		{
			if (!ModelState.IsValid)
				return RedirectToAction("Edit");
			
			var store = _storeFactory.PrepareStore(_storeContext.CurrentStore, model);
			_storeService.Update(store);
			
			return RedirectToAction("Edit");
		}
	}
}
