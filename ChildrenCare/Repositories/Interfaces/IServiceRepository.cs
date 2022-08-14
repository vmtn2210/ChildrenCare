using ChildrenCare.DTOs.ReservationDetailDTOs;
using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Repositories.Interfaces;

public interface IServiceRepository : IRepositoryBase<Service>
{
    Task<PagedList<GetServiceListResponseDTO>> AdvancedGetServices(AdvancedGetServiceRequestDTO dto);
    Task<GetFeaturedServiceListResponseDTO> GetFeatureServiceList();
    Task<bool> ServiceExists(int serviceId);
    Task<GetInfoForCreateNewReservationDetailDTO?> GetInfoForCreateNewReservationDetail(int serviceId);
}