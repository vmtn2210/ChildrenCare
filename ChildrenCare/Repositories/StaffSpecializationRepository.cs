using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class StaffSpecializationRepository : RepositoryBase<StaffSpecialization>, IStaffSpecializationRepository
{
    public StaffSpecializationRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}