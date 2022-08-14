using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChildrenCare.Utilities;

namespace ChildrenCare.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        [Required]
        public virtual AppUser CustomerAccount { get; set; }
        
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        public bool CustomerGender { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [RegularExpression(GlobalVariables.PhoneNumberRegex)]
        public string PhoneNumber { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [MaxLength(500)]
        public string? Address { get; set; }

        [Required]
        public long TotalBasePrice { get; set; }

        [Required]
        public long ActualTotalPrice { get; set; }
        
        public DateTime? PreservedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<ReservationDetail> ReservationDetails { get; set; }
        
        public virtual ICollection<Prescription> Prescriptions { get; set; }

    }
}
