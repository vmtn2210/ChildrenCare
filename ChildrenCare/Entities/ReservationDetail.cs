using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChildrenCare.Entities;

public class ReservationDetail
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Reservation")]
    public int ReservationId { get; set; }

    [Required]
    public virtual Reservation Reservation { get; set; }
    
    [ForeignKey("Staff")]
    public int? StaffId { get; set; }
    
    public virtual AppUser? Staff { get; set; }

    [Required]
    [ForeignKey("Service")]
    public int ServiceId { get; set; }

    [Required]
    public virtual Service Service { get; set; }

    [Required]
    public int NumberOfPeople { get; set; }

    [Required]
    public int Amount { get; set; }

    [Required]
    public long BasePrice { get; set; }
    
    public long? SalePrice { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
}