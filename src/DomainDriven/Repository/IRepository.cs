using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainDriven.Specifications;

namespace DomainDriven.Repository
{
    public interface IRepository : IDisposable
    {
        Task<T> Get<T>(object id) where T : class, IPersistentObject;
        Task<IEnumerable<T>> GetAll<T>() where T : class, IPersistentObject;
        Task<PagedResult<T>> Get<T>(int pageNumber, int itemsPerPage) where T : class, IPersistentObject;
        Task<T> GetEditable<T>(object id) where T : class, IPersistentObject;
        Task<IEnumerable<T>> FindEditable<T>(Specification<T> specification) where T : class, IPersistentObject;
        Task<IEnumerable<T>> Find<T>(Specification<T> specification) where T : class, IPersistentObject;
        Task<IEnumerable<T>> Find<T>(Specification<T> specification, IList<ISortField> sortFields) where T : class, IPersistentObject;
        Task<PagedResult<T>> Find<T>(Specification<T> specification, IList<ISortField> sortFields, int pageNumber, int itemsPerPage) where T : class, IPersistentObject;
        // Task<IEnumerable<T>> Find<T>(IQuery<T> query) where T : class, IPersistentObject;
        Task<T> FirstOrDefault<T>(Specification<T> specification) where T : class, IPersistentObject;
        Exception LastException { get; }
        // Task Execute(ICommand command);
        void Add<T>(T value) where T : class, IPersistentObject;
        void Remove<T>(T value) where T : class, IPersistentObject;
        Task SaveChanges();
    }
}