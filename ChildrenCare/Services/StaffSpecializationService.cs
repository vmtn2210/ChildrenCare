using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;

namespace ChildrenCare.Services;

public class StaffSpecializationService : IStaffSpecializationService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public StaffSpecializationService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<StaffSpecialization>> GetStaffSpecializationList(IEnumerable<int> serviceIds)
    {
        var result =
            await _repositoryWrapper.StaffSpecialization.FindByConditionAsyncWithInclude(
                x => serviceIds.Contains(x.ServiceId), x => x.Staff);

        return result;
    }

    public async Task<StaffSpecialization?> GetFirstStaffByServiceId(int serviceId)
    {
        var result =
            await _repositoryWrapper.StaffSpecialization.FindSingleByConditionAsync(x => x.ServiceId == serviceId);

        return result;
    }

    public async Task<IEnumerable<StaffSpecialization>> GetStaffSpecializationList()
    {
        var result =
            await _repositoryWrapper.StaffSpecialization.FindByConditionAsyncWithMultipleIncludes(x => true,
                x => x.Service,
                x => x.Staff);

        return result;
    }

    public async Task Delete(int serviceId, int staffId)
    {
        var staffSpecialization =
            await _repositoryWrapper.StaffSpecialization.FindSingleByConditionAsync(x =>
                x.ServiceId == serviceId && x.StaffId == staffId);
        if (staffSpecialization != null)
            await _repositoryWrapper.StaffSpecialization.DeleteAsync(staffSpecialization);
    }

    public async Task Create(int serviceId, int staffId)
    {
        var staffSpecialization =
            await _repositoryWrapper.StaffSpecialization.FindSingleByConditionAsync(x =>
                x.ServiceId == serviceId && x.StaffId == staffId);
        if (staffSpecialization == null)
            await _repositoryWrapper.StaffSpecialization.CreateAsync(new StaffSpecialization()
            {
                ServiceId = serviceId,
                StaffId = staffId
            });
    }
}