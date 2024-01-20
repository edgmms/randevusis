using Hyper.Core;
using Hyper.Core.Domain.Organisations;
using Hyper.Services.Authentication;
using Hyper.Services.Organisations;

namespace Hyper.Web.Infrastructure
{
    public class WebStoreContext : IStoreContext
    {
        private Store _store;
        private readonly IAuthenticationService _authenticationService;
        private readonly IStoreService _storeService;

        public WebStoreContext(IAuthenticationService authenticationService, IStoreService storeService)
        {
            _authenticationService = authenticationService;
            _storeService = storeService;
        }

        public Store CurrentStore
        {
            get
            {
                if (_store is not null)
                    return _store;

                var applicationUser = _authenticationService.GetAuthenticatedApplicationUser();
                _store = _storeService.GetById(applicationUser.RegisteredInStoreId);
                return _store;
            }
        }
    }
}
