using System.Globalization;
using ChildrenCare.Data;
using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Text;

namespace ChildrenCare.Repositories;

public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
{
    private readonly ChildrenCareDBContext _cdbContext;

    public ReservationRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
        _cdbContext = cdbContext;
    }

    public async Task<int?> GetCurrentReservationId(int userId)
    {
        return await _cdbContext.Reservations.Where(reservation =>
                reservation.CustomerAccountId == userId && reservation.Status == GlobalVariables.DraftReservationStatus)
            .Select(reservation => reservation.Id)
            .SingleOrDefaultAsync();
    }

    public async Task<GetInfoForCreateNewReservationDTO> GetInfoForCreateNewReservation(int userId, int serviceId)
    {
        var result = new GetInfoForCreateNewReservationDTO
        {
            Customer = await _cdbContext.Users.Where(user => user.Id == userId).Select(user =>
                new GetInfoForCreateNewReservationCustomerDTO()
                {
                    CustomerName = user.FullName,
                    CustomerEmail = user.Email,
                    CustomerGender = user.Gender,
                    CustomerPhoneNumber = user.PhoneNumber
                }).FirstOrDefaultAsync(),
            Service = await _cdbContext.Services.Where(service => service.Id == serviceId).Select(service =>
                new GetInfoForCreateNewReservationServiceDTO()
                {
                    ServicePrice = service.Price,
                    ServiceSalePrice = service.SalePrice,
                }).FirstOrDefaultAsync()
        };

        return result;
    }

    public async Task<bool> HasOngoingReservation(int userId)
    {
        return await _cdbContext.Reservations.Where(reservation =>
                reservation.CustomerAccountId == userId &&
                reservation.Status != GlobalVariables.DraftReservationStatus &&
                reservation.Status != GlobalVariables.CancelledReservationStatus &&
                reservation.Status != GlobalVariables.SuccessReservationStatus &&
                reservation.Status != GlobalVariables.SubmittedReservationStatus)
            .AnyAsync();
    }

    public async Task<PagedList<GetMyReservationListReservationDTO>> GetMyReservationList(
        GetMyReservationListRequestDTO dto)
    {
        var query = _cdbContext.Reservations.Where(reservation => reservation.CustomerAccountId == dto.UserId)
            .Include(reservation => reservation.ReservationDetails).Select(
                reservation => new GetMyReservationListReservationDTO()
                {
                    Id = reservation.Id,
                    Status = reservation.Status,
                    CreatedDate = reservation.CreatedDate,
                    LastUpdate = reservation.LastUpdate,
                    PreservedDate = reservation.PreservedDate,
                    ServiceNumber = reservation.ReservationDetails.Count,
                    ActualTotalPrice = reservation.ActualTotalPrice,
                    TotalBasePrice = reservation.ActualTotalPrice
                }).AsQueryable();

        //If there's status filtering
        if (dto.FilterStatuses != null && dto.FilterStatuses.Any())
        {
            query = query.Where(reservationDto =>
                dto.FilterStatuses.Select(status => status).Contains(reservationDto.Status));
        }

        //If there Id filtering
        if (dto.FilterIds != null && dto.FilterIds.Any())
        {
            query = query.Where(reservationDto =>
                dto.FilterIds.Contains(reservationDto.Id));
        }

        try
        {
            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MinCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.CreatedDate >= date);
            }

            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MaxCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.CreatedDate <= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MinLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.LastUpdate >= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MaxLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.LastUpdate <= date);
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
                "id_asc" => query.OrderBy(reservationDto => reservationDto.Id),
                "id_desc" => query.OrderByDescending(reservationDto => reservationDto.Id),
                "status_asc" => query.OrderBy(reservationDto => reservationDto.Status),
                "status_desc" => query.OrderByDescending(reservationDto => reservationDto.Status),
                "createddate_asc" => query.OrderBy(reservationDto => reservationDto.CreatedDate),
                "createddate_desc" => query.OrderByDescending(reservationDto => reservationDto.CreatedDate),
                "lastupdate_asc" => query.OrderBy(reservationDto => reservationDto.LastUpdate),
                "lastupdate_desc" => query.OrderByDescending(reservationDto => reservationDto.LastUpdate),
                _ => query
            };
        }

        return await PagedList<GetMyReservationListReservationDTO>.CreateAsync(
            query.Select(reservationDto => reservationDto), dto.PageNumber,
            dto.PageSize);
    }

    public async Task<GetMyReservationListReservationDTO?> GetReservationById(int userId, int reservationId,
        string role)
    {
        var query = _cdbContext.Reservations
            .Where(reservation => (reservation.CustomerAccountId == userId || "doctor".Equals(role) || "nurse".Equals(role)) &&
                                  reservation.Id == reservationId)
            .Include(reservation => reservation.ReservationDetails).Select(
                reservation => new GetMyReservationListReservationDTO()
                {
                    Id = reservation.Id,
                    Status = reservation.Status,
                    CreatedDate = reservation.CreatedDate,
                    LastUpdate = reservation.LastUpdate,
                    PreservedDate = reservation.PreservedDate,
                    ServiceNumber = reservation.ReservationDetails.Count,
                    ActualTotalPrice = reservation.ActualTotalPrice,
                    TotalBasePrice = reservation.ActualTotalPrice,
                    CustomerEmail = reservation.CustomerEmail
                }).AsQueryable();

        return await query.Select(reservationDto => reservationDto).FirstOrDefaultAsync();
    }

    public async Task<PagedList<AdvancedGetReservationListReservationDTO>> AdvancedGetReservationList(
        AdvancedGetReservationRequestDTO dto)
    {
        var query = _cdbContext.Reservations.Include(reservation => reservation.ReservationDetails).Select(
            reservation => new AdvancedGetReservationListReservationDTO()
            {
                Id = reservation.Id,
                Status = reservation.Status,
                CreatedDate = reservation.CreatedDate,
                LastUpdate = reservation.LastUpdate,
                PreservedDate = reservation.PreservedDate,
                ServiceNumber = reservation.ReservationDetails.Count,
                ActualTotalPrice = reservation.ActualTotalPrice,
                TotalBasePrice = reservation.ActualTotalPrice,
                CustomerId = reservation.CustomerAccountId,
                CustomerName = reservation.CustomerName
            }).AsQueryable();

        //If there's status filtering
        if (dto.FilterStatuses != null && dto.FilterStatuses.Any())
        {
            query = query.Where(reservationDto =>
                dto.FilterStatuses.Select(status => status).Contains(reservationDto.Status));
        }

        //If there Id filtering
        if (dto.FilterIds != null && dto.FilterIds.Any())
        {
            query = query.Where(reservationDto =>
                dto.FilterIds.Contains(reservationDto.Id));
        }

        try
        {
            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MinCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.CreatedDate >= date);
            }

            //If there's CreatedDate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxCreatedDate))
            {
                var date = DateTime.ParseExact(dto.MaxCreatedDate, GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.CreatedDate <= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MinLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MinLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.LastUpdate >= date);
            }

            //If there's LastUpdate Filtering
            if (!string.IsNullOrWhiteSpace(dto.MaxLastUpdate))
            {
                var date = DateTime.ParseExact(dto.MaxLastUpdate,
                    GlobalVariables.DateTimeFormat,
                    CultureInfo.InvariantCulture);
                query = query.Where(reservationDto => reservationDto.LastUpdate <= date);
            }

            if (!string.IsNullOrWhiteSpace(dto.CustomerName))
            {                
                query = query.Where(reservationDto => reservationDto.CustomerName.Contains(dto.CustomerName));
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
                "id_asc" => query.OrderBy(reservationDto => reservationDto.Id),
                "id_desc" => query.OrderByDescending(reservationDto => reservationDto.Id),
                "status_asc" => query.OrderBy(reservationDto => reservationDto.Status),
                "status_desc" => query.OrderByDescending(reservationDto => reservationDto.Status),
                "createddate_asc" => query.OrderBy(reservationDto => reservationDto.CreatedDate),
                "createddate_desc" => query.OrderByDescending(reservationDto => reservationDto.CreatedDate),
                "lastupdate_asc" => query.OrderBy(reservationDto => reservationDto.LastUpdate),
                "lastupdate_desc" => query.OrderByDescending(reservationDto => reservationDto.LastUpdate),
                _ => query
            };
        }

        return await PagedList<AdvancedGetReservationListReservationDTO>.CreateAsync(
            query.Select(reservationDto => reservationDto), dto.PageNumber,
            dto.PageSize);
    }
}