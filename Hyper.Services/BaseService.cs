using Hyper.Core;
using Hyper.Core.Domain;
using Hyper.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyper.Services
{
    public partial class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity<int>
    {
        private readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public virtual TEntity GetById(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            return _repository.GetById(id);
        }

        /// <summary>
        /// The GetByIdAsync.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task{TEntity}"/>.</returns>
        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            return _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// The GetByIds.
        /// </summary>
        /// <param name="ids">The ids<see cref="int[]"/>.</param>
        /// <returns>The <see cref="Task{List{TEntity}}"/>.</returns>
        public virtual List<TEntity> GetByIds(int[] ids)
        {
            if (ids is null)
                throw new ArgumentNullException("ids");

            return _repository.Table.Where(x => ids.Contains(x.Id)).ToList();
        }

        /// <summary>
        /// The GetAllPagedList
        /// </summary>
        /// <param name="pageIndex">The pageIdex <see cref="int"/>.</param>
        /// <param name="pageSize">The pageSize <see cref="int"/>.</param>
        /// <returns>The <see cref="IPagedList{TEntity}"/>.</returns>
        public virtual IPagedList<TEntity> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _repository.Table;
            var result = new PagedList<TEntity>(query, pageIndex, pageSize);
            return result;
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Insert(entity);
        }


        /// <summary>
        /// The InsertRange.
        /// </summary>
        /// <param name="entities">The entity<see cref="List{TEntity}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual void InsertRange(List<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entity");

            if (entities.Any())
            {
                _repository.InsertRange(entities);
            }
        }

        /// <summary>
        /// The Insert.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            await _repository.InsertAsync(entity);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Update(entity);
        }

        /// <summary>
        /// The UpdateAsync.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            await _repository.UpdateAsync(entity);
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Delete(entity);
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="entity">The entity<see cref="TEntity"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            await _repository.DeleteAsync(entity);
        }

        /// <summary>
        /// The DeleteById.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual void DeleteById(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            var entity = this.GetById(id);
            if (entity is not null)
                this.Delete(entity);
        }

        /// <summary>
        /// The DeleteById.
        /// </summary>
        /// <param name="id">The id<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public virtual async Task DeleteByIdAsync(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            var entity = await this.GetByIdAsync(id);
            if (entity is not null)
                await this.DeleteAsync(entity);
        }
    }
}
