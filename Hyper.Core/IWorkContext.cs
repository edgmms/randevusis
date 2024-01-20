using Hyper.Core.Domain.ApplicationUsers;

namespace Hyper.Core
{
    public partial interface IWorkContext
    {
        ApplicationUser CurrentApplicationUser { get; }
    
		bool IsAdministrator { get; }

        bool IsStoreAdministrator { get; }

		bool IsStoreUser { get; }
	}
}
