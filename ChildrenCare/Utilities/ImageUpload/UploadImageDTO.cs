using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.Utilities.ImageUpload
{
    public class UploadImageDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}