using System.ComponentModel.DataAnnotations;
using ChildrenCare.Entities;

namespace ChildrenCare.DTOs.SliderDTOs;

public class UpdateSliderRequestDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public string BackLink { get; set; }

    [Required]
    public string ThumbnailUrl { get; set; }

    public Slider MapAndUpdateSlider(Slider slider, string thumbnail = "")
    {
        var hasChanged = false;

        if (!string.IsNullOrWhiteSpace(Title) && Title != slider.Title)
        {
            slider.Title = Title;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(BackLink))
        {
            slider.BackLink = BackLink;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(ThumbnailUrl))
        {
            slider.ThumbnailUrl = ThumbnailUrl;
            hasChanged = true;
        }

        if (Status >= 0 && Status != slider.Status)
        {
            slider.Status = Status;
        }

        if (hasChanged)
        {
            slider.LastUpdate = DateTime.Now;
        }
        else
        {
            //TODO: Handle Exception
        }

        return slider;
    }
}