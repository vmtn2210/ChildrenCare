using ChildrenCare.Entities;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.ReservationDTOs;


    public class AdvancedGetReservationListReservationDTO
    {
        public int Id { get; set; }
        public long TotalBasePrice { get; set; }
        public long ActualTotalPrice { get; set; }
        public DateTime? PreservedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int ServiceNumber { get; set; }
        public int Status { get; set; }
        
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
    
public class AdvancedGetReservationResponseDTO
{
        public PagedList<AdvancedGetReservationListReservationDTO>? Reservations { get; set; }
        public IEnumerable<AppParameter>? AppParameters { get; set; }
    }