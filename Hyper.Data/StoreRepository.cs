using Hyper.Core.Domain;
using Hyper.Core.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hyper.Data
{
    public partial class StoreRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        #region Fields
        private readonly IHyperDbProvider _hyperDatabaseProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private DbSet<TEntity> _entities;
        private readonly int _currentApplicationUserId = 0;
        private readonly int _currentApplicationUserStoreId = 0;
        private readonly bool _storeCheck = false;
        #endregion

        #region Ctor

        public StoreRepository(IHyperDbProvider hyperDatabaseProvider,
             IHttpContextAccessor httpContextAccessor)
        {
            _hyperDatabaseProvider = hyperDatabaseProvider;
            _httpContextAccessor = httpContextAccessor;
            _entities = _hyperDatabaseProvider.DbContext.Set<TEntity>();

            //try to get authenticated user identity
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(HyperAuthenticationDefaults.AuthenticationScheme).Result;
            if (authenticateResult.Succeeded)
            {
                _storeCheck = true;

                var idClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == HyperAuthenticationDefaults.ApplicationUserIdClaim
                && claim.Issuer.Equals(HyperAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                _currentApplicationUserId = Convert.ToInt32(idClaim.Value);

                var storeIdClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == HyperAuthenticationDefaults.ApplicationUserStoreClaim
              && claim.Issuer.Equals(HyperAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                _currentApplicationUserStoreId = Convert.ToInt32(storeIdClaim.Value);
            }
        }

        #endregion

        #region Methods

        public TEntity GetById(int id)
        {
            if (id < 0)
                throw new ArgumentNullException(nameof(id));

            var query = this.Table;

            query = query.Where(x => x.Id == id);

            return query.FirstOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            if (id < 0)
                throw new ArgumentNullException(nameof(id));

            var query = this.Table;
         
            query = query.Where(x => x.Id == id);
            
            return await query.FirstOrDefaultAsync();
        }

        public void Insert(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.CreatedOnUtc = DateTime.UtcNow;
                    fullAuditedEntity.CreatedById = _currentApplicationUserId;
                    break;

                default:
                    break;
            }

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            _entities.Add(entity);
            _hyperDatabaseProvider.DbContext.SaveChanges();
        }

        public async Task InsertAsync(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.CreatedOnUtc = DateTime.UtcNow;
                    fullAuditedEntity.CreatedById = _currentApplicationUserId;
                    break;

                default:
                    break;
            }

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            await _entities.AddAsync(entity);
            await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<FullAuditedEntity<int>>().Any())
            {
                foreach (var entity in entities)
                {
                    if (entity is FullAuditedEntity<int> fullAuditedEntity)
                    {
                        fullAuditedEntity.CreatedOnUtc = DateTime.UtcNow;
                        fullAuditedEntity.CreatedById = _currentApplicationUserId;
                    }

                    if (_storeCheck && entity is IStoreEntity storeEntity)
                        storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;
                }
            }

            _entities.AddRange(entities);
            _hyperDatabaseProvider.DbContext.SaveChanges();
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<FullAuditedEntity<int>>().Any())
            {
                foreach (var entity in entities)
                {
                    if (entity is FullAuditedEntity<int> fullAuditedEntity)
                    {
                        fullAuditedEntity.CreatedOnUtc = DateTime.UtcNow;
                        fullAuditedEntity.CreatedById = _currentApplicationUserId;
                    }

                    if (_storeCheck && entity is IStoreEntity storeEntity)
                        storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;
                }
            }

            await _entities.AddRangeAsync(entities);
            await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.UpdatedById = _currentApplicationUserId;
                    fullAuditedEntity.UpdatedOnUtc = DateTime.UtcNow;
                    break;

                default:
                    break;
            }

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            _entities.Update(entity);
            _hyperDatabaseProvider.DbContext.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            switch (entity)
            {
                case null:
                    throw new ArgumentNullException(nameof(entity));

                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.UpdatedById = _currentApplicationUserId;
                    fullAuditedEntity.UpdatedOnUtc = DateTime.UtcNow;
                    break;

                default:
                    break;
            }

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            _entities.Update(entity);
            await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<FullAuditedEntity<int>>().Any())
            {
                foreach (var entity in entities)
                {
                    if (entity is FullAuditedEntity<int> fullAuditedEntity)
                    {
                        fullAuditedEntity.CreatedById = _currentApplicationUserId;
                        fullAuditedEntity.CreatedOnUtc = DateTime.UtcNow;
                    }

                    if (_storeCheck && entity is IStoreEntity storeEntity)
                        storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;
                }
            }

            _entities.AddRange(entities);
            _hyperDatabaseProvider.DbContext.SaveChanges();
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            if (entities.OfType<FullAuditedEntity<int>>().Any())
            {
                foreach (var entity in entities)
                {
                    if (entity is FullAuditedEntity<int> fullAuditedEntity)
                    {
                        fullAuditedEntity.UpdatedById = _currentApplicationUserId;
                        fullAuditedEntity.UpdatedOnUtc = DateTime.UtcNow;
                    }

                    if (_storeCheck && entity is IStoreEntity storeEntity)
                        storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;
                }
            }

            await _entities.AddRangeAsync(entities);
            await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            switch (entity)
            {
                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.Deleted = true;
                    fullAuditedEntity.DeletedById = _currentApplicationUserId;
                    fullAuditedEntity.DeletedOnUtc = DateTime.UtcNow;
                    break;

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    this.Update(entity);
                    break;

                default:
                    _entities.Remove(entity);
                    _hyperDatabaseProvider.DbContext.SaveChanges();
                    break;
            }

        }

        public void Delete(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            var entity = this.GetById(id);

            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            switch (entity)
            {
                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.Deleted = true;
                    fullAuditedEntity.DeletedById = _currentApplicationUserId;
                    fullAuditedEntity.DeletedOnUtc = DateTime.UtcNow;
                    break;

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    this.Update(entity);
                    break;

                default:
                    _entities.Remove(entity);
                    _hyperDatabaseProvider.DbContext.SaveChanges();
                    break;
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            switch (entity)
            {
                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.Deleted = true;
                    fullAuditedEntity.DeletedById = _currentApplicationUserId;
                    fullAuditedEntity.DeletedOnUtc = DateTime.UtcNow;
                    break;

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    await this.UpdateAsync(entity);
                    break;

                default:
                    _entities.Remove(entity);
                    await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
                    break;
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0)
                throw new ArgumentNullException("id");

            var entity = await this.GetByIdAsync(id);

            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (_storeCheck && entity is IStoreEntity storeEntity)
                storeEntity.RegisteredInStoreId = _currentApplicationUserStoreId;

            switch (entity)
            {
                case FullAuditedEntity<int> fullAuditedEntity:
                    fullAuditedEntity.Deleted = true;
                    fullAuditedEntity.DeletedById = _currentApplicationUserId;
                    fullAuditedEntity.DeletedOnUtc = DateTime.UtcNow;
                    break;

                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.Deleted = true;
                    await this.UpdateAsync(entity);
                    break;

                default:
                    _entities.Remove(entity);
                    await _hyperDatabaseProvider.DbContext.SaveChangesAsync();
                    break;
            }
        }

        #endregion

        #region Properties

        public IQueryable<TEntity> Table
        {
            get
            {
                var query = _entities.AsQueryable();

                if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) != null)
                    query = query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();

                if (_storeCheck && typeof(TEntity).GetInterface(nameof(IStoreEntity)) != null)
                    query = query.OfType<IStoreEntity>().Where(x => x.RegisteredInStoreId == _currentApplicationUserStoreId).OfType<TEntity>();

                return query;
            }
        }

        #endregion

    }
}
