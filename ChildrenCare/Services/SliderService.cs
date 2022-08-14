using ChildrenCare.DTOs.SliderDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.ImageUpload;

namespace ChildrenCare.Services;

public class SliderService : ISliderService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IPhotoService _photoService;

    public SliderService(IRepositoryWrapper repositoryWrapper, IPhotoService photoService)
    {
        _repositoryWrapper = repositoryWrapper;
        _photoService = photoService;
    }
    public async Task<IEnumerable<Slider>> SearchSlider(string status, string title, string backlink)
    {
        return await _repositoryWrapper.Slider.FindByConditionAsync(r => r.Status.ToString().Contains(status) && r.Title.Contains(title) && r.BackLink.Contains(backlink));
    }

    public async Task<CustomResponse> CreateSlider(CreateSliderRequestDTO dto)
    {

        string? thumbnailUrl = null;

        if (dto.Thumbnail != null)
        {
            var imageUploadResult = await _photoService.AppPhotoAsync(dto.Thumbnail);
            //If there's error
            if (imageUploadResult.Error != null)
            {
                //TODO: Throw Exception
            }

            thumbnailUrl = imageUploadResult.SecureUrl.AbsoluteUri;
        }

        var newSlider = dto.ToNewSlider(thumbnailUrl);

        try
        {
            await _repositoryWrapper.Slider.CreateAsync(newSlider);
        }
        catch (Exception e)
        {
            //clear pending changes if fail
            _repositoryWrapper.DeleteChanges();
            //TODO: Log Exception

            return new CustomResponse(false, e.Message);
        }

        return new CustomResponse(true, "Slider Created");
    }
}