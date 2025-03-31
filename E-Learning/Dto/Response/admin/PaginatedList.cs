using Microsoft.EntityFrameworkCore;

namespace E_Learning.Dto.Response.admin
{
    public class PaginatedList<T>
    {
        public List<T> Content { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool First => PageNumber == 1;
        public bool Last => PageNumber == TotalPages;
        public bool Empty => Content.Count == 0;
        public int NumberOfElements => Content.Count;

        public List<SortInfo> Sort => new List<SortInfo>
    {
        new SortInfo
        {
            Direction = "DESC",
            Property = "createdAt",
            IgnoreCase = false,
            NullHandling = "NATIVE",
            Ascending = false,
            Descending = true
        }
    };

        public PageableInfo Pageable => new PageableInfo
        {
            PageNumber = PageNumber - 1,
            PageSize = PageSize,
            Offset = (PageNumber - 1) * PageSize,
            Paged = true,
            Unpaged = false,
            Sort = Sort
        };

        private PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Content = items;
            TotalElements = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
        // ✅ Dùng khi source là IEnumerable đã materialized (ToList, v.v.)
        public static Task<PaginatedList<T>> CreateFromEnumerableAsync(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize));
        }
    }



    public class SortInfo
    {
        public string Direction { get; set; }
        public string Property { get; set; }
        public bool IgnoreCase { get; set; }
        public string NullHandling { get; set; }
        public bool Ascending { get; set; }
        public bool Descending { get; set; }
    }

    public class PageableInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Offset { get; set; }
        public bool Paged { get; set; }
        public bool Unpaged { get; set; }
        public List<SortInfo> Sort { get; set; }
    }

}
