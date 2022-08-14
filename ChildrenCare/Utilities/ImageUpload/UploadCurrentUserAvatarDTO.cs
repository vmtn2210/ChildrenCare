using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.Utilities.ImageUpload
{
    public class UploadCurrentUserAvatarDTO
    {
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}