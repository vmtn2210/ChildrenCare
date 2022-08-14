using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Utilities.Pagination
{
    //Used for pagination
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        
        public IEnumerable<T> Items { get; set; }
        
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages =(int) Math.Ceiling(count/(double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            Items = items;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            //Get Total number of item from database
            var count = await source.CountAsync();
            //Get the items based on page size and page number
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync() ;
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}