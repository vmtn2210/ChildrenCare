using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class AppRoleRepository : RepositoryBase<AppRole>, IAppRoleRepository
{
    public AppRoleRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}