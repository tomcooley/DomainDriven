using System.Collections;
using System.Collections.Generic;

namespace DomainDriven.Repository
{
    public class PagedResult<T> : IEnumerable<T>, IPagedResult<T>
    {
        private readonly IList<T> _results;

        public PagedResult(IList<T> results, int totalCount, int skipped, int itemsPerPage)
        {
            _results = results;
            TotalResults = totalCount;
            ItemsPerPage = itemsPerPage;
            Page = (int)((decimal)skipped / itemsPerPage);
            TotalPages = (int)((decimal)totalCount / ItemsPerPage + 1);
        }

        public int TotalResults { get; private set; }

        /// <summary>
        /// Page Number is 0 based
        /// </summary>
        public int Page { get; private set; }
        public int TotalPages { get; private set; }
        public int ItemsPerPage { get; private set; }

        public int Count
        {
            get { return _results.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        public T this[int index]
        {
            get { return _results[index]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}