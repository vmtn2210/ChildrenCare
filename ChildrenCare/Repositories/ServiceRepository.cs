using System.Globalization;
using System.Linq;
using ChildrenCare.Data;
using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.DTOs.ReservationDetailDTOs;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Repositories;

public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
{
    private readonly ChildrenCareDBContext _cdbContext;

    public ServiceRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
        _cdbContext = cdbContext;
    }

    public async Task<PagedList<GetServiceListResponseDTO>> AdvancedGetServices(AdvancedGetServiceRequestDTO dto)
    {
        var query = _cdbContext.Services.Select(b => new GetServiceListResponseDTO()
        {
            Id = b.Id,
            Status = b.Status,
            Name = b.Name,
            ThumbnailUrl = b.ThumbnailUrl,
            CreatedDate = b.CreatedDate,
            LastUpdated = b.LastUpdated,
            BriefInfo = b.BriefInfo,
            SalePrice = b.SalePrice,
            Price = b.Price,
        }).AsQueryable();

        //If there's status filtering
        if (dto.FilterStatuses != null && dto.FilterStatuses.Any())
        {
            query = query.Where(article =>
                dto.FilterStatuses.Contains(article.Status));
        }

        //If there Id filtering
        if (dto.FilterIds != null && dto.FilterIds.Any())
        {
            query = query.Where(blog =>
                dto.FilterIds.Contains(blog.Id));
        }

        //If There's Tittle Filtering
        if (!string.IsNullOrWhiteSpace(dto.FilterName))
        {
            query = query.Where(blog => blog.Name.ToLower().Contains(dto.FilterName.ToLower()));
        }

        if (dto.MaxPrice >= 0)
        {
            query = query.Where(service => service.Price <= dto.MaxPrice);
        }
        if (dto.MinPrice >= 0)
        {
            query = query.Where(service => service.Price >= dto.MinPrice);
        }

        if (dto.MaxSalePrice >= 0)
        {
            query = query.Where(service => service.SalePrice <= dto.MaxSalePrice);
        }

        if (dto.MinSalePrice >= 0)
        {
            query = query.Where(service => service.SalePrice >= dto.MinSalePrice);
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
                query = query.Where(blog => blog.LastUpdated >= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MaxLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(blog => blog.LastUpdated <= date);
            }
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                query = query.Where(blog => blog.Name.Contains(dto.Title));
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
                "tittle_asc" => query.OrderBy(blog => blog.Name),
                "tittle_desc" => query.OrderByDescending(blog => blog.Name),
                "status_asc" => query.OrderBy(blog => blog.Status),
                "status_desc" => query.OrderByDescending(blog => blog.Status),
                "createddate_asc" => query.OrderBy(blog => blog.CreatedDate),
                "createddate_desc" => query.OrderByDescending(blog => blog.CreatedDate),
                "lastupdate_asc" => query.OrderBy(blog => blog.LastUpdated),
                "lastupdate_desc" => query.OrderByDescending(blog => blog.LastUpdated),
                _ => query
            };
        }

        return await PagedList<GetServiceListResponseDTO>.CreateAsync(
            query.Select(service => service), dto.PageNumber,
            dto.PageSize);
    }

    public async Task<GetFeaturedServiceListResponseDTO> GetFeatureServiceList()
    {
        var resultList = await _cdbContext.Services.Where(service => service.IsFeatured).Select(service =>
            new FeaturedServiceResponseDTO()
            {
                Id = service.Id,
                Name = service.Name,
                BriefInfo = service.BriefInfo,
                ThumbnailUrl = service.ThumbnailUrl
            }).ToListAsync();
        return new GetFeaturedServiceListResponseDTO(resultList);
    }

    public async Task<bool> ServiceExists(int serviceId)
    {
        return await _cdbContext.Services.Where(service => service.Id == serviceId).AnyAsync();
    }

    public async Task<GetInfoForCreateNewReservationDetailDTO?> GetInfoForCreateNewReservationDetail(int serviceId)
    {
        return await _cdbContext.Services.Where(service => service.Id == serviceId)
            .Select(service => new GetInfoForCreateNewReservationDetailDTO()
            {
                ServiceSalePrice = service.SalePrice,
                ServicePrice = service.Price
            }).SingleOrDefaultAsync();
    }
}