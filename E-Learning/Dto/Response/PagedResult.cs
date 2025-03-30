using Microsoft.EntityFrameworkCore;

namespace E_Learning.Dto.Response
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalItems { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }

        public PagedResult(IEnumerable<T> items, int totalItems, int currentPage, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
