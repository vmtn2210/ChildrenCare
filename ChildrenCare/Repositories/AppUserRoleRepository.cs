using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class AppUserRoleRepository : RepositoryBase<AppUserRole>, IAppUserRoleRepository
{
    public AppUserRoleRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}