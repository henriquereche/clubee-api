using Clubee.API.Models.Filters.Base;
using System.Collections.Generic;

namespace Clubee.API.Models.Base
{
    /// <summary>
    /// Base application response for listing operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T>
    {
        public readonly long TotalCount;
        public readonly int CurrentPage;
        public readonly int PageSize;
        public readonly IEnumerable<T> Items;

        /// <summary>
        /// Create a new ListResult from items, count and paging.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        public ListResult(
            IEnumerable<T> items,
            long totalCount,
            int currentPage,
            int pageSize
            )
        {
            this.Items = items;
            this.TotalCount = totalCount;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Create a new ListResult from items, count and baseFilter.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <param name="filter"></param>
        public ListResult(
            IEnumerable<T> items,
            long totalCount,
            BaseFilter filter
        ) : this(
            items,
            totalCount,
            filter.Page,
            filter.PageSize
        ) { }
    }
}
