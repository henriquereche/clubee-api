namespace Clubee.API.Models.Filters.Base
{
    public class BaseFilter
    {
        /// <summary>
        /// Current page.
        /// </summary>
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }
        private int _page = 1;


        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 1 : value >= 100 ? 100 : value;
        }
        private int _pageSize = 10;
    }
}
