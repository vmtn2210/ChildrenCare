using ChildrenCare.Entities;

namespace ChildrenCare.Services.Interface;

public interface IStaffSpecializationService
{
    Task<IEnumerable<StaffSpecialization>> GetStaffSpecializationList(IEnumerable<int> serviceIds);
    Task<StaffSpecialization?> GetFirstStaffByServiceId(int serviceId);
    Task<IEnumerable<StaffSpecialization>> GetStaffSpecializationList();
    Task Delete(int serviceId, int staffId);
    Task Create(int serviceId, int staffId);
}