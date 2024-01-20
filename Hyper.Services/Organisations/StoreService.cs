using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Core.Domain.Organisations;
using Hyper.Data;
using Hyper.Services.ApplicationUsers;
using System.Collections.Generic;
using System.Linq;

namespace Hyper.Services.Organisations
{
	public class StoreService : BaseService<Store>, IStoreService
	{
		private readonly IRepository<Store> _storeRepository;
		private readonly IApplicationUserService _applicationUserService;

		public StoreService(IRepository<Store> storeRepository, IApplicationUserService applicationUserService) : base(storeRepository)
		{
			_storeRepository = storeRepository;
			_applicationUserService = applicationUserService;

		}
	}
}