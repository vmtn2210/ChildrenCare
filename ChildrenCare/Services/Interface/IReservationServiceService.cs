using ChildrenCare.Entities;
using ChildrenCare.Utilities;

namespace ChildrenCare.Services.Interface;

public interface IReservationServiceService
{
    Task<IEnumerable<ReservationDetail>> GetReservationDetailList(int reservationId);
    Task<CustomResponse> AddServiceAmount(int detailId);
    Task<CustomResponse> DecreaseServiceAmount(int detailId);
    Task<CustomResponse> RecalculateTotal(int reservationId);
    Task<CustomResponse> RemoveServiceFromReservation(int detailId);
    Task<CustomResponse> AddServiceNumberOfPeople(int detailId);
    Task<CustomResponse> DecreaseServiceNumberOfPeople(int detailId);
    Task<CustomResponse> AssignStaffToService(int detailId, int staffId);
    Task<Reservation> FindById(int? id);
}