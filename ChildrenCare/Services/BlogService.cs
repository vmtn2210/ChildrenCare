using System.Net;
using System.Security.Claims;
using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.ImageUpload;
using ChildrenCare.Utilities.Pagination;
using Microsoft.AspNetCore.Identity;

namespace ChildrenCare.Services
{
    public class BlogService : IBlogService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public BlogService(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _repositoryWrapper = repositoryWrapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _photoService = photoService;
        }

        public async Task<BlogDetailDTO?> GetBlog(int id)
        {
            //return await _repositoryWrapper.Blog.FindSingleByConditionAsync(blog => blog.Id == id);
            return await _repositoryWrapper.Blog.GetBlogDetail(id);
        }

        public async Task<PagedList<GetBlogListResponseDTO>> GetBlogList(GetBlogListRequestDTO dto)
        {
            var result = await _repositoryWrapper.Blog.GetBlogList(dto);
            return result;
        }

        public async Task<AdvancedGetBlogCustomerListResponseDTO> GetBlogList(string? category, string? title)
        {
            var result = new AdvancedGetBlogCustomerListResponseDTO()
            {
                Blogs = await _repositoryWrapper.Blog.GetBlogList(
                new GetBlogListRequestDTO()
                {
                    SortBy = "createddate_desc",
                    Category = category,
                    Title = title
                }),
                AppParameters = await _repositoryWrapper.Parameter.FindByConditionAsync(parameter =>
                    parameter.Type == GlobalVariables.ReservationStatusParameterType)
            };

            return result;
        }



        //public async Task<PagedList<GetBlogManagementListResponseDTO>> GetBlogManagementList(GetBlogManagementListRequestDTO dto)
        //{
        //    var result = await _repositoryWrapper.Blog.GetBlogManagementList(dto);
        //    return result;
        //}

        public async Task<AdvancedGetBlogListResponseDTO> GetBlogManagementList(string category, string status, string title)
        {
            var result = new AdvancedGetBlogListResponseDTO()
            {
                Blogs = await _repositoryWrapper.Blog.GetBlogManagementList(
                new GetBlogManagementListRequestDTO()
                {
                    SortBy = "createddate_desc",
                    Category = category,
                    Status = status,
                    Title = title
                }),
                AppParameters = await _repositoryWrapper.Parameter.FindByConditionAsync(parameter =>
                    parameter.Type == GlobalVariables.ReservationStatusParameterType)
            };

            return result;
        }


        public async Task<CustomResponse> CreateBlog(CreateBlogRequestDTO dto)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId < 0)
            {
                return new CustomResponse(false, "Current User is not logged in");
            }

            string? avatarUrl = null;

            if (dto.ThumbnailFile != null)
            {
                var imageUploadResult = await _photoService.AppPhotoAsync(dto.ThumbnailFile);
                //If there's error
                if (imageUploadResult.Error != null)
                {
                    //TODO: Throw Exception
                }

                avatarUrl = imageUploadResult.SecureUrl.AbsoluteUri;
            }

            var newBlog = dto.ToNewBlog(currentUserId, avatarUrl);
            
            try
            {
                await _repositoryWrapper.Blog.CreateAsync(newBlog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //clear pending changes if fail
                _repositoryWrapper.DeleteChanges();
                //TODO: Log Exception

                return new CustomResponse(false, "Blog cannot be created. Please input fully!");
            }

            return new CustomResponse(true,"Blog Created");
        }

        public async Task<BlogDetailDTO?> GetBlogDetail(int id)
        {
            return await _repositoryWrapper.Blog.GetBlogDetail(id);
        }

        public async Task<IEnumerable<Blog>> GetBlogManagementList()
        {
            return await _repositoryWrapper.Blog.FindAllAsync();
        }

        private int GetCurrentUserId()
        {
            var currentUserId = -1;
            if (_httpContextAccessor.HttpContext == null)
            {
                //TODO: Throw Exception
            }

            try
            {
                //Get Current user Id
                currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            catch (ArgumentNullException e)
            {
                //TODO:Throw Exception
            }

            return currentUserId;
        }

        public async Task<IEnumerable<Blog>> GetBlogList()
        {
            return await _repositoryWrapper.Blog.FindAllAsync();
        }
    }
}
