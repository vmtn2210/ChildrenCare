using System.Globalization;
using System.Security.Claims;
using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Entities;

namespace ChildrenCare.Services;

public class ReservationService : IReservationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public ReservationService(IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repositoryWrapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<Reservation>> SearchReservationByName(string name)
    {
        return await _repositoryWrapper.Reservation.FindByConditionAsync(r =>
            r.CustomerName.Contains(name) || r.Status.ToString().Equals(name));
    }

    public async Task<GetMyReservationListResponseDTO> GetMyReservationList()
    {
        var result = new GetMyReservationListResponseDTO()
        {
            Reservations = await _repositoryWrapper.Reservation.GetMyReservationList(
                new GetMyReservationListRequestDTO()
                {
                    UserId = GetCurrentUserId(),
                    SortBy = "createddate_desc"
                }),
            AppParameters = await _repositoryWrapper.Parameter.FindByConditionAsync(parameter =>
                parameter.Type == GlobalVariables.ReservationStatusParameterType)
        };

        return result;
    }

    public async Task<GetMyReservationListReservationDTO?> GetReservationById(int reservationId)
    {
        return await _repositoryWrapper.Reservation.GetReservationById(GetCurrentUserId(), reservationId,
            GetCurrentUserRole());
    }

    public async Task<AdvancedGetReservationResponseDTO> AdvancedGetReservationList(string customerName)
    {
        var result = new AdvancedGetReservationResponseDTO()
        {
            Reservations = await _repositoryWrapper.Reservation.AdvancedGetReservationList(
                new AdvancedGetReservationRequestDTO()
                {
                    SortBy = "createddate_desc",
                    CustomerName = customerName
                }),
            AppParameters = await _repositoryWrapper.Parameter.FindByConditionAsync(parameter =>
                parameter.Type == GlobalVariables.ReservationStatusParameterType)
        };

        return result;
    }

    public async Task<UpdateReservationContactDTO> GetReservationContactInfo(int reservationId)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == reservationId);
        return new UpdateReservationContactDTO()
        {
            Address = reservation?.Address,
            Notes = reservation?.Address,
            Id = reservationId,
            CustomerName = reservation?.CustomerName,
            PhoneNumber = reservation?.PhoneNumber,
            PreservedDate = reservation?.PreservedDate,
            CustomerGender = reservation?.CustomerGender == true
                ? (int)GlobalVariables.GenderType.Male
                : (int)GlobalVariables.GenderType.Female
        };
    }

    public async Task<CustomResponse> SubmitReservation(UpdateReservationContactDTO dto)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == dto.Id);
        var servicesNotAssign =
            await _repositoryWrapper.ReservationService.FindByConditionAsync(x =>
                x.ReservationId == dto.Id && x.StaffId == null);

        if (reservation == null)
        {
            return new CustomResponse(false, "Reservation Not Found");
        }

        if (reservation.CustomerName != dto.CustomerName)
        {
            reservation.CustomerName = dto.CustomerName ?? reservation.CustomerName;
        }

        if (reservation.Address != dto.Address)
        {
            reservation.Address = dto.Address;
        }

        var gender = dto.CustomerGender == (int)GlobalVariables.GenderType.Male;

        if (reservation.CustomerGender != gender)
        {
            reservation.CustomerGender = !reservation.CustomerGender;
        }

        if (reservation.Notes != dto.Notes)
        {
            reservation.Notes = dto.Notes;
        }

        if (reservation.PreservedDate != dto.PreservedDate && dto.PreservedDate >= DateTime.Now)
        {
            try
            {
                reservation.PreservedDate = dto.PreservedDate;
            }
            catch (Exception e)
            {
                return new CustomResponse(false, "Cannot select date time in the past. Try again");
            }
        }
        
        reservation.LastUpdate = DateTime.Now;

        reservation.Status = GlobalVariables.SubmittedReservationStatus;

        try
        {
            await _repositoryWrapper.Reservation.UpdateAsync(reservation, dto.Id);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error Happened when updating reservation");
        }

        if (servicesNotAssign.Any())
        {
            var serviceIds = servicesNotAssign.Select(x => x.ServiceId);
            var staffSpecializations = await
                _repositoryWrapper.StaffSpecialization.FindByConditionAsync(x => serviceIds.Contains(x.ServiceId));
            foreach (var service in servicesNotAssign)
            {
                var staff = staffSpecializations.FirstOrDefault(x => x.ServiceId == service.ServiceId);
                if (staff == null) continue;
                service.StaffId = staff.StaffId;
                await _repositoryWrapper.ReservationService.UpdateAsync(service, service.Id);
            }
        }

        return new CustomResponse(true, "Reservation Submitted");
    }

    public async Task<CustomResponse> ApproveReservation(int id)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == id);
        if (reservation == null)
        {
            return new CustomResponse(false, "Reservation Not Found");
        }

        reservation.Status = GlobalVariables.ApprovedReservationStatus;
        try
        {
            await _repositoryWrapper.Reservation.UpdateAsync(reservation, id);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error Happened when updating reservation");
        }

        return new CustomResponse(true, "Reservation Approved");
    }

    public async Task<CustomResponse> CancelReservation(int id)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == id);
        if (reservation == null)
        {
            return new CustomResponse(false, "Reservation Not Found");
        }

        reservation.Status = GlobalVariables.CancelledReservationStatus;
        try
        {
            await _repositoryWrapper.Reservation.UpdateAsync(reservation, id);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error Happened when updating reservation");
        }

        return new CustomResponse(true, "Reservation Cancelled");
    }

    public async Task<CustomResponse> DeleteReservation(int id)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == id);
        if (reservation == null)
        {
            return new CustomResponse(false, "Reservation Not Found");
        }

        try
        {
            if (reservation.Status == GlobalVariables.DraftReservationStatus)
            {
                await _repositoryWrapper.Reservation.DeleteAsync(reservation);
            }
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error Happened when updating reservation");
        }

        return new CustomResponse(true, "Reservation Cancelled");
    }

    public async Task<CustomResponse> ReservationSuccess(int id)
    {
        var reservation = await _repositoryWrapper.Reservation.FindSingleByConditionAsync(r => r.Id == id);
        if (reservation == null)
        {
            return new CustomResponse(false, "Reservation Not Found");
        }

        reservation.Status = GlobalVariables.SuccessReservationStatus;
        try
        {
            await _repositoryWrapper.Reservation.UpdateAsync(reservation, id);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error Happened when updating reservation");
        }

        return new CustomResponse(true, "Reservation Success");
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

    private string? GetCurrentUserRole()
    {
        string? currentUserRole = null;
        try
        {
            //Get Current user Id
            currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
        catch (ArgumentNullException e)
        {
            //TODO:Throw Exception
        }

        return currentUserRole;
    }
}