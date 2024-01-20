using Microsoft.EntityFrameworkCore;

namespace Hyper.Data
{
    public partial interface IHyperDbProvider
    {
        /// <summary>
        /// Gets the DbContext.
        /// </summary>
        DbContext DbContext { get; }
    }
}
