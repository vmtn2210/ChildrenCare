using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;

namespace ChildrenCare.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public PrescriptionService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<IEnumerable<Prescription>> FindByReservationId(int reservationId)
    {
        var result = await _repositoryWrapper.Prescription
            .FindByConditionAsync(x => x.ReservationId == reservationId);
        return result;
    }

    public async Task<IEnumerable<Prescription>> FindAll()
    {
        var result = await _repositoryWrapper.Prescription
            .FindAllAsync();
        return result;
    }
}