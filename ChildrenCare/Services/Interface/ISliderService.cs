using ChildrenCare.DTOs.SliderDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;

namespace ChildrenCare.Services.Interface;

public interface ISliderService
{
    public Task<IEnumerable<Slider>> SearchSlider(string status, string title, string backlink);
}