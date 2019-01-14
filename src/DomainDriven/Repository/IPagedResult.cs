using System.Collections.Generic;

namespace DomainDriven.Repository
{
    public interface IPagedResult<T>
    {
        int TotalResults { get; }

        /// <summary>
        /// Page Number is 0 based
        /// </summary>
        int Page { get; }

        int TotalPages { get; }
        int ItemsPerPage { get; }
        int Count { get; }
        IEnumerator<T> GetEnumerator();
        T this[int index] { get; }
    }
}