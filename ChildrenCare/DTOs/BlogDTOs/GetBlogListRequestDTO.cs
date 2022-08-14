using System.Collections.ObjectModel;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.BlogDTOs
{
    public class GetBlogListRequestDTO : PaginationParams
    {
        public GetBlogListRequestDTO()
        {
        }

        public static ReadOnlyCollection<string> OrderingParams { get; } = new ReadOnlyCollection<string>(
            new string[] {
                "id_asc", "id_desc",
                "tittle_asc", "tittle_desc",
            }
        );

        public IEnumerable<int> FilterIds { get; set; }
        public string FilterTittle { get; set; }
        public string SortBy { get; set; }

        public string Category { get; set; }
        public string Title { get; set; }
    }
}
