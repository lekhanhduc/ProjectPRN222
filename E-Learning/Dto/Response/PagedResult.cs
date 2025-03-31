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

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, string sortBy = "createdAt", string sortDir = "asc")
        {
            // Đảm bảo có OrderBy trước khi sử dụng Skip/Take
            if (sortBy.ToLower() == "createdat")
            {
                source = sortDir.ToLower() == "desc"
                    ? source.OrderByDescending(p => EF.Property<object>(p, "CreatedAt"))
                    : source.OrderBy(p => EF.Property<object>(p, "CreatedAt"));
            }
            else
            {
                source = source.OrderBy(p => EF.Property<object>(p, "CreatedAt")); // Mặc định sắp xếp theo CreatedAt nếu không có sortBy
            }

            // Đếm tổng số bản ghi
            var count = await source.CountAsync();

            // Lấy các bản ghi trong phạm vi phân trang
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Trả về kết quả phân trang
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }

    }
}
