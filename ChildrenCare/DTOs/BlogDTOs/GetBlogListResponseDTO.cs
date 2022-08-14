using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.BlogDTOs;

public class GetBlogListResponseDTO
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Tittle { get; set; }
    public string BriefInfo { get; set; }
    public int Status { get; set; }
    public string ThumbnailUrl { get; set; }
}

public class AdvancedGetBlogCustomerListResponseDTO
{
    public PagedList<GetBlogListResponseDTO>? Blogs { get; set; }
    public IEnumerable<AppParameter>? AppParameters { get; set; }
}