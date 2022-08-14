using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.Entities;

public class Slider
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter title")]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int Status { get; set; }

    [Required(ErrorMessage = "Please enter backlink of the image")]
    public string BackLink { get; set; }

    [Required(ErrorMessage = "Please enter thumbnail link")]
    public string ThumbnailUrl { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastUpdate { get; set; }

}