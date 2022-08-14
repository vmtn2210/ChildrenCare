using CloudinaryDotNet.Actions;

namespace ChildrenCare.Utilities.ImageUpload
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AppPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}