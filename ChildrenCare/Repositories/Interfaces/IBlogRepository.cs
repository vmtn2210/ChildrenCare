using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Repositories.Interfaces
{
    public interface IBlogRepository : IRepositoryBase<Blog>
    {
        Task<PagedList<Blog>> AdvancedGetBlogs();
        Task<PagedList<GetBlogManagementListResponseDTO>> GetBlogManagementList(GetBlogManagementListRequestDTO dto);
        Task<PagedList<GetBlogListResponseDTO>> GetBlogList(GetBlogListRequestDTO dto);
        Task<BlogDetailDTO?> GetBlogDetail(int id);
    }
}
