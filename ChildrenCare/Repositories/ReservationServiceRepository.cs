using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Repositories;

public class ReservationServiceRepository : RepositoryBase<ReservationDetail>, IReservationServiceRepository
{
    public ReservationServiceRepository(ChildrenCareDBContext cdbContext) : base(cdbContext)
    {
    }
}