using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Services.Interface;

public interface IServiceService
{
    Task<CustomResponse> CreateService(CreateServiceRequestDTO dto);
    Task<GetFeaturedServiceListResponseDTO> GetFeaturedServiceList();
    Task<PagedList<GetServiceListResponseDTO>> AdvancedGetServiceList(AdvancedGetServiceRequestDTO dto);
    Task<CustomResponse> AddService(int serviceId);
    public Task<IEnumerable<Service>> GetAll();
    public Task<IEnumerable<Service>> SearchService(string status, string title, string briefInfo);
}