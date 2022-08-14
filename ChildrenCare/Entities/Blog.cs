using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChildrenCare.Entities
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Author")]
        public int AuthorUserId { get; set; }

        [Required]
        public virtual AppUser Author { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(255)]
        public string Tittle { get; set; }

        [Required]
        [MaxLength(255)]
        public string BriefInfo { get; set; }

        [Required]
        [MaxLength(50)]
        public int Status { get; set; }

        [Required]
        public string BlogBody { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
