using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainDriven.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DomainDriven.Repository
{
    public class Repository : IRepository
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public Exception LastException => throw new NotImplementedException();

        public void Add<T>(T value) where T : class, IPersistentObject
        {
            _dbContext.Set<T>().Add(value);
        }

        // public async Task Execute(ICommand command)
        // {
            // await _dbContext.Database.ExecuteSqlCommandAsync()
        // }

        public async Task<IEnumerable<T>> Find<T>(Specification<T> specification) where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().Where(specification).ToListAsync();
        }

        public async Task<IEnumerable<T>> Find<T>(Specification<T> specification, IList<ISortField> sortFields) where T : class, IPersistentObject
        {
            return await Query(specification, sortFields).ToListAsync();
        }

        public async Task<PagedResult<T>> Find<T>(Specification<T> specification, IList<ISortField> sortFields, int pageNumber, int itemsPerPage) where T : class, IPersistentObject
        {
            var skip = (pageNumber - 1) * itemsPerPage;
            var results = await Query(specification, sortFields).Skip(skip).Take(itemsPerPage).ToListAsync();

            var itemCount = _dbContext.Set<T>().Count(specification);

            return new PagedResult<T>(results, itemCount, skip, itemsPerPage);
        }

        // public async Task<IEnumerable<T>> Find<T>(IQuery<T> query) where T : class, IPersistentObject
        // {
            // throw new NotImplementedException();
        // }

        public async Task<IEnumerable<T>> FindEditable<T>(Specification<T> specification) where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().AsTracking().Where(specification).ToListAsync();
        }

        public async Task<T> FirstOrDefault<T>(Specification<T> specification) where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(specification);
        }

        public async Task<T> Get<T>(object id) where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> GetEditable<T>(object id) where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().AsTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Remove<T>(T value) where T : class, IPersistentObject
        {
            _dbContext.Set<T>().Remove(value);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<T>> Get<T>(int pageNumber, int itemsPerPage) where T : class, IPersistentObject
        {
            var skip = (pageNumber - 1) * itemsPerPage;
            var results = await _dbContext.Set<T>().Skip(skip).Take(itemsPerPage).ToListAsync();

            var itemCount = _dbContext.Set<T>().Count();

            return new PagedResult<T>(results, itemCount, skip, itemsPerPage);
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : class, IPersistentObject
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        private IQueryable<T> Query<T>(Specification<T> specification, IList<ISortField> sortFields = null) where T : class, IPersistentObject
        {
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            var query = _dbContext.Set<T>().Where(specification);

            if (sortFields != null && sortFields.Any())
            {
                for (int i = 0; i < sortFields.Count; i++)
                {
                    var field = sortFields[i];
                    query = i == 0
                        ? query.OrderBy(field.SortField, field.SortDirection)
                        : query.ThenBy(field.SortField, field.SortDirection);
                }
            }
            return query;
        }
        #region IDisposable

        // Flag: Has Dispose already been called?
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual async void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                await SaveChanges();
            }

            // Free any unmanaged objects here.
            //
            _disposed = true;
        }

        #endregion

    }
}