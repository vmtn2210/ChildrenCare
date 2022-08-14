using System.ComponentModel.DataAnnotations;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;

namespace ChildrenCare.DTOs.SliderDTOs;

public class CreateSliderRequestDTO
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public string BackLink { get; set; }

    [Required]
    public IFormFile Thumbnail { get; set; }

    public Slider ToNewSlider(string thumbnail = "")
    {
        var result = new Slider()
        {
            Title = Title,
            Status = GlobalVariables.ActiveSliderStatusId,
            BackLink = BackLink,
            ThumbnailUrl = thumbnail,
            CreatedDate = DateTime.Now,
            LastUpdate = DateTime.Now
        };
        return result;
    }
}