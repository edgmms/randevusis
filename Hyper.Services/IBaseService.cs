using Hyper.Core;
using Hyper.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyper.Services
{
    /// <summary>
    /// Defines the <see cref="IBaseService{TEntity}" />.
    /// </summary>
    /// <typeparam name="TEntity">.</typeparam>
    public partial interface IBaseService<TEntity> where TEntity : BaseEntity<int>
    {
        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        TEntity GetById(int id);

        /// <summary>
        /// The GetByIdAsync.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// The GetByIds.
        /// </summary>
        /// <param name="ids">The ids<see cref="int[]"/>.</param>
        /// <returns>The <see cref="Task{List{TEntity}}"/>.</returns>
        List<TEntity> GetByIds(int[] ids);

        /// <summary>
        /// The GetAllPagedList
        /// </summary>
        /// <param name="pageIndex">The pageIdex <see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize <see cref="int"/>.</param>
        /// <returns>The <see cref="IPagedList{TEntity}"/>.</returns>
        IPagedList<TEntity> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        void Insert(TEntity entity);

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entities">The entity<see cref="List{TEntity}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        void InsertRange(List<TEntity> entities);

        /// <summary>
        /// The InsertAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        void Update(TEntity entity);

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        void Delete(TEntity entity);

        /// <summary>
        /// The DeleteAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// The DeleteById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        void DeleteById(int id);

        /// <summary>
        /// The DeleteByIdAsync.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task DeleteByIdAsync(int id);
    }
}
