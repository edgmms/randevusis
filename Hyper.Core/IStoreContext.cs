using Hyper.Core.Domain.Organisations;

namespace Hyper.Core
{
    public interface IStoreContext
    {
        Store CurrentStore { get; }

        //Branch CurrentBranch { get; set; }
    }
}
