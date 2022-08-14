using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Repositories.Interfaces;

public interface IReservationRepository : IRepositoryBase<Reservation>
{
    Task<bool> HasOngoingReservation(int userId);
    Task<int?> GetCurrentReservationId(int userId);
    Task<GetInfoForCreateNewReservationDTO> GetInfoForCreateNewReservation(int userId, int serviceId);

    Task<PagedList<GetMyReservationListReservationDTO>> GetMyReservationList(
        GetMyReservationListRequestDTO dto);

    Task<GetMyReservationListReservationDTO?> GetReservationById(
        int userId, int reservationId, string role);

    Task<PagedList<AdvancedGetReservationListReservationDTO>> AdvancedGetReservationList(AdvancedGetReservationRequestDTO dto);
}