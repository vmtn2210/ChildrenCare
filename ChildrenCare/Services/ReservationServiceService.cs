using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;

namespace ChildrenCare.Services;

public class ReservationServiceService : IReservationServiceService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public ReservationServiceService(IHttpContextAccessor httpContextAccessor, IRepositoryWrapper repositoryWrapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _repositoryWrapper = repositoryWrapper;
    }


    public async Task<IEnumerable<ReservationDetail>> GetReservationDetailList(int reservationId)
    {
        var result = await
            _repositoryWrapper.ReservationService.FindByConditionAsyncWithInclude(
                detail => detail.ReservationId == reservationId && detail.Amount > 0, detail => detail.Service);
        return result;
    }

    public async Task<CustomResponse> AddServiceAmount(int detailId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        reservationDetail.Amount++;
        try
        {
            await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
        }

        return new CustomResponse(true, "ReservationDetail amount increased");
    }

    public async Task<CustomResponse> DecreaseServiceAmount(int detailId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        if (reservationDetail.Amount >= 2)
        {
            reservationDetail.Amount--;
            try
            {
                await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
            }
            catch (Exception e)
            {
                return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
            }
        }

        return new CustomResponse(true, "ReservationDetail amount increased");
    }

    public async Task<CustomResponse> RecalculateTotal(int reservationId)
    {
        var reservation =
            await _repositoryWrapper.Reservation.FindSingleByConditionAsync(reservation =>
                reservation.Id == reservationId);
        if (reservation == null)
            return new CustomResponse(false, "Reservation not found");
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindByConditionAsync(detail => detail.ReservationId == reservationId);
        var actualTotal = 0L;
        var baseTotal = 0L;
        foreach (var detail in reservationDetail)
        {
            var actualPrice = detail.SalePrice ?? detail.BasePrice;
            actualTotal += detail.Amount * actualPrice;
            baseTotal += detail.Amount * detail.BasePrice;
        }

        reservation.ActualTotalPrice = actualTotal;
        reservation.TotalBasePrice = baseTotal;
        await _repositoryWrapper.Reservation.UpdateAsync(reservation, reservationId);
        return new CustomResponse(true, "Reservation amount updated");
    }

    public async Task<CustomResponse> RemoveServiceFromReservation(int detailId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        if (reservationDetail.Amount != 0)
        {
            reservationDetail.Amount = 0;
            try
            {
                await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
            }
            catch (Exception e)
            {
                return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
            }
        }

        return new CustomResponse(true, "ReservationDetail removed");
    }

    public async Task<CustomResponse> AddServiceNumberOfPeople(int detailId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        reservationDetail.NumberOfPeople++;
        try
        {
            await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
        }

        return new CustomResponse(true, "ReservationDetail number of people increased");
    }

    public async Task<CustomResponse> DecreaseServiceNumberOfPeople(int detailId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        if (reservationDetail.NumberOfPeople >= 2)
        {
            reservationDetail.NumberOfPeople--;
            try
            {
                await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
            }
            catch (Exception e)
            {
                return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
            }
        }

        return new CustomResponse(true, "ReservationDetail amount increased");
    }

    public async Task<CustomResponse> AssignStaffToService(int detailId, int staffId)
    {
        var reservationDetail = await
            _repositoryWrapper.ReservationService.FindSingleByConditionAsync(detail => detail.Id == detailId);
        if (reservationDetail == null)
        {
            return new CustomResponse(false, "ReservationDetail not found");
        }

        reservationDetail.StaffId = staffId;
        try
        {
            await _repositoryWrapper.ReservationService.UpdateAsync(reservationDetail, detailId);
        }
        catch (Exception e)
        {
            return new CustomResponse(false, "Internal Error happened when updating Reservation Detail");
        }

        return new CustomResponse(true, "ReservationDetail assigned");
    }

    public async Task<Reservation> FindById(int? id)
    {
        return await _repositoryWrapper.Reservation.FindSingleByConditionAsync(x =>x.Id == id);
    }
}