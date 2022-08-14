using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Services.Interface
{
    public interface IBlogService
    {
        Task<BlogDetailDTO?> GetBlog(int id);
        Task<PagedList<GetBlogListResponseDTO>> GetBlogList(GetBlogListRequestDTO dto);
        Task<AdvancedGetBlogCustomerListResponseDTO> GetBlogList(string? category, string? title);
        //Task<PagedList<GetBlogManagementListResponseDTO>> GetBlogManagementList(GetBlogManagementListRequestDTO dto);
        Task<AdvancedGetBlogListResponseDTO> GetBlogManagementList(string? category, string? status, string? title);
        Task<CustomResponse> CreateBlog(CreateBlogRequestDTO dto);
        Task<BlogDetailDTO?> GetBlogDetail(int id);
        //public Task<IEnumerable<Blog>> SearchBlogManagement(string category, string author, string status);
    }
}
