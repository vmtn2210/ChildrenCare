using System.ComponentModel.DataAnnotations;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Utilities;

namespace ChildrenCare.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter service name")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter brief info")]
        [MaxLength(255)]
        public string BriefInfo { get; set; }

        [Required(ErrorMessage = "Please enter price (VNĐ)")]
        public long Price { get; set; }

        public long? SalePrice { get; set; }

        [Required]
        public int Status { get; set; }
        [Required(ErrorMessage = "Please enter thumbnail link")]
        public string ThumbnailUrl { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [Required]
        public bool IsFeatured { get; set; }
        
    }
}
