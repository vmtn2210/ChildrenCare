using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using ChildrenCare.Data;
using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Repositories;

public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
{
    private readonly ChildrenCareDBContext _cdbContext;
    public BlogRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
        _cdbContext = cdbContext;
    }

    public Task<PagedList<Blog>> AdvancedGetBlogs()
    {
        throw new NotImplementedException();
    }

    public async Task<PagedList<GetBlogManagementListResponseDTO>> GetBlogManagementList(GetBlogManagementListRequestDTO dto)
    {
        var query = _cdbContext.Blogs.Include(blog => blog.Author).Select(b => new GetBlogManagementListResponseDTO()
        {
            Id = b.Id,
            AuthorUserId = b.AuthorUserId,
            AuthorName = b.Author.FullName,
            CategoryId = b.CategoryId,
            Status = b.Status,
            Tittle = b.Tittle,
            ThumbnailUrl = b.ThumbnailUrl,
            CreatedDate = b.CreatedDate,
            LastUpdate = b.LastUpdate
        }).AsQueryable();

        //If there's status filtering
        if (dto.FilterStatuses != null && dto.FilterStatuses.Any())
        {
            query = query.Where(article =>
                dto.FilterStatuses.Select(status => status).Contains(article.Status));
        }

        //If there Id filtering
        if (dto.FilterIds != null && dto.FilterIds.Any())
        {
            query = query.Where(blog =>
                dto.FilterIds.Contains(blog.Id));
        }

        //If there AuthorId filtering
        if (dto.AuthorUserIds != null && dto.AuthorUserIds.Any())
        {
            query = query.Where(blog =>
                dto.AuthorUserIds.Contains(blog.AuthorUserId));
        }

        //If There's Tittle Filtering
        if (!string.IsNullOrWhiteSpace(dto.FilterTittle))
        {
            query = query.Where(blog => blog.Tittle.ToLower().Contains(dto.FilterTittle.ToLower()));
        }

        try
        {
            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MinCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(blog => blog.CreatedDate >= date);
            }

            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MaxCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(blog => blog.CreatedDate <= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MinLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(blog => blog.LastUpdate >= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MaxLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(blog => blog.LastUpdate <= date);
            }
            if (!string.IsNullOrWhiteSpace(dto.Category))
            {
                query = query.Where(blog => blog.CategoryId == int.Parse(dto.Category));
            }
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                query = query.Where(blog => blog.Tittle.Contains(dto.Title));
            }
            if (!string.IsNullOrWhiteSpace(dto.Status))
            {
                query = query.Where(blog => blog.Status == int.Parse(dto.Status));
            }
        }
        catch (FormatException)
        {
            //TODO: Handle Exception;
        }


        //If there's sorting
        if (!string.IsNullOrWhiteSpace(dto.SortBy))
        {
            query = dto.SortBy switch
            {
                "id_asc" => query.OrderBy(blog => blog.Id),
                "id_desc" => query.OrderByDescending(blog => blog.Id),
                "authoruserid_asc" => query.OrderBy(blog => blog.AuthorUserId),
                "authoruserid_desc" => query.OrderByDescending(blog => blog.AuthorUserId),
                "authorname_asc" => query.OrderBy(blog => blog.AuthorName),
                "authorname_desc" => query.OrderByDescending(blog => blog.AuthorName),
                "tittle_asc" => query.OrderBy(blog => blog.Tittle),
                "tittle_desc" => query.OrderByDescending(blog => blog.Tittle),
                "status_asc" => query.OrderBy(blog => blog.Status),
                "status_desc" => query.OrderByDescending(blog => blog.Status),
                "createddate_asc" => query.OrderBy(blog => blog.CreatedDate),
                "createddate_desc" => query.OrderByDescending(blog => blog.CreatedDate),
                "lastupdate_asc" => query.OrderBy(blog => blog.LastUpdate),
                "lastupdate_desc" => query.OrderByDescending(blog => blog.LastUpdate),
                _ => query
            };
        }

        return await PagedList<GetBlogManagementListResponseDTO>.CreateAsync(
            query.Select(blog => blog), dto.PageNumber,
            dto.PageSize);
    }

    public async Task<PagedList<GetBlogListResponseDTO>> GetBlogList(GetBlogListRequestDTO dto)
    {
        var query = _cdbContext.Blogs.Include(blog => blog.Author).Select(b => new GetBlogListResponseDTO()
        {
            Id = b.Id,
            CategoryId = b.CategoryId,
            Status = b.Status,
            Tittle = b.Tittle,
            ThumbnailUrl = b.ThumbnailUrl,
            BriefInfo = b.BriefInfo
        }).AsQueryable();

        //If there Id filtering
        if (dto.FilterIds != null && dto.FilterIds.Any())
        {
            query = query.Where(blog =>
                dto.FilterIds.Contains(blog.Id));
        }

        //If There's Tittle Filtering
        if (!string.IsNullOrWhiteSpace(dto.FilterTittle))
        {
            query = query.Where(blog => blog.Tittle.ToLower().Contains(dto.FilterTittle.ToLower()));
        }

        //If there's sorting
        if (!string.IsNullOrWhiteSpace(dto.SortBy))
        {
            query = dto.SortBy switch
            {
                "id_asc" => query.OrderBy(blog => blog.Id),
                "id_desc" => query.OrderByDescending(blog => blog.Id),
                "tittle_asc" => query.OrderBy(blog => blog.Tittle),
                "tittle_desc" => query.OrderByDescending(blog => blog.Tittle),
                "status_asc" => query.OrderBy(blog => blog.Status),
                "status_desc" => query.OrderByDescending(blog => blog.Status),
                _ => query
            };
        }
        if (!string.IsNullOrWhiteSpace(dto.Category))
        {
            query = query.Where(blog => blog.CategoryId == int.Parse(dto.Category));
        }
        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            query = query.Where(blog => blog.Tittle.Contains(dto.Title));
        }

        return await PagedList<GetBlogListResponseDTO>.CreateAsync(
            query.Select(blog => blog), dto.PageNumber,
            dto.PageSize);
    }

    public async Task<BlogDetailDTO?> GetBlogDetail(int id)
    {
        return await _cdbContext.Blogs.Where(blog => blog.Id == id).Include(blog => blog.Category)
            .Include(blog => blog.Author).Select(blog => new BlogDetailDTO()
            {
                Id = blog.Id,
                BriefInfo = blog.BriefInfo,
                CategoryId = blog.CategoryId,
                Tittle = blog.Tittle,
                ThumbnailUrl = blog.ThumbnailUrl,
                Status = blog.Status,
                BlogBody = blog.BlogBody,
                AuthorName = blog.Author.FullName,
                AuthorUserId = blog.AuthorUserId,
                CreatedDate = blog.CreatedDate,
                LastUpdate = blog.LastUpdate,
                CategoryName = blog.Category.Name
            }).FirstOrDefaultAsync();
    }
}