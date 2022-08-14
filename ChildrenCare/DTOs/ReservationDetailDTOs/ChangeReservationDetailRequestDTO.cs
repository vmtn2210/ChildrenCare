using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.DTOs.ReservationDetailDTOs;

public class ChangeReservationDetailRequestDTO
{
    [Required]
    public int DetailId { get; set; }
    
    [Required]
    public int ReservationId { get; set; }
}