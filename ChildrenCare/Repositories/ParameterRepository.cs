using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class ParameterRepository : RepositoryBase<AppParameter>, IParameterRepository
{
    private readonly ChildrenCareDBContext _cdbContext;
    public ParameterRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
        _cdbContext = cdbContext;
    }
}