using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories;
using ChildrenCare.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairCutAppAPI.Repositories;

public class UserRepository : RepositoryBase<AppUser>, IUserRepository
{
    private readonly ChildrenCareDBContext _cdbContext;

    public UserRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
        _cdbContext = cdbContext;
    }
}