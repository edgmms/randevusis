using Hyper.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyper.Data
{
    /// <summary>
    /// Defines the <see cref="IRepository{TEntity}" />.
    /// </summary>
    /// <typeparam name="TEntity">.</typeparam>
    public partial interface IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        TEntity GetById(int id);
        
        Task<TEntity> GetByIdAsync(int id);
        
        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);

        Task InsertRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(int id);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(int id);

        IQueryable<TEntity> Table { get; }
    }
}
