using ChildrenCare.Entities;

namespace ChildrenCare.Services.Interface;

public interface IPrescriptionService
{
    Task<IEnumerable<Prescription>> FindByReservationId(int id);

    Task<IEnumerable<Prescription>> FindAll();
}