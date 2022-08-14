using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class PrescriptionRepository : RepositoryBase<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}