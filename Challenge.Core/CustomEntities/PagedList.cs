using System;
using System.Collections.Generic;
using System.Linq;

namespace Challenge.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PagedList(List<T>items, int count, int pageNumber, int pageSize)
        {

            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pagenumber = 1, int pageSize= 10)
        {
            var count = source.Count();
            var items = source.Skip((pagenumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, pagenumber, pageSize);
        }
    }
}
