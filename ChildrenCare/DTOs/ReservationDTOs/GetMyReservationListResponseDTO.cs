using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.ReservationDTOs
{
    public class GetMyReservationListReservationDTO
    {
        public int Id { get; set; }
        public long TotalBasePrice { get; set; }
        public long ActualTotalPrice { get; set; }
        public DateTime? PreservedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int ServiceNumber { get; set; }
        public int Status { get; set; }
        public string? CustomerEmail { get; set; }
    }

    public class GetMyReservationListResponseDTO
    {
        public PagedList<GetMyReservationListReservationDTO>? Reservations { get; set; }
        public IEnumerable<AppParameter>? AppParameters { get; set; }
    }
}