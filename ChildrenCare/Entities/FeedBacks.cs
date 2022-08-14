using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChildrenCare.Entities;

public class FeedBacks
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("ReservationService")]
    public int ReservationServiceId { get; set; }

    [Required]
    public virtual ReservationDetail? ReservationService { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public int Star { get; set; }

    public string? Comment { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
}