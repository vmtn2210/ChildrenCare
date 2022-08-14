using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.Utilities;
using ChildrenCare.Entities;

namespace ChildrenCare.Services.Interface;

public interface IReservationService
{
    Task<GetMyReservationListResponseDTO> GetMyReservationList();
    Task<GetMyReservationListReservationDTO?> GetReservationById(int reservationId);
    Task<UpdateReservationContactDTO> GetReservationContactInfo(int reservationId);
    Task<CustomResponse> SubmitReservation(UpdateReservationContactDTO dto);
    Task<AdvancedGetReservationResponseDTO> AdvancedGetReservationList(string? customerName);
    Task<CustomResponse> ApproveReservation(int id);
    Task<CustomResponse> CancelReservation(int id);
    Task<CustomResponse> DeleteReservation(int id);
    Task<CustomResponse> ReservationSuccess(int id);
    Task<IEnumerable<Reservation>> SearchReservationByName(string name);
}