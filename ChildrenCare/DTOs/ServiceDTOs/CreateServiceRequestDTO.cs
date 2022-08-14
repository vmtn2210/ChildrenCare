using System.ComponentModel.DataAnnotations;
using ChildrenCare.Entities;
using ChildrenCare.Utilities;

namespace ChildrenCare.DTOs.ServiceDTOs;

public class CreateServiceRequestDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string Description { get; set; }

    [MaxLength(255)]
    public string BriefInfo { get; set; }

    [Required]
    public long Price { get; set; }

    public long? SalePrice { get; set; }

    [Required]
    [MaxLength(50)]
    public int Status { get; set; }
    public IFormFile? ThumbnailFile { get; set; }

    public Service ToNewService(string thumbnailUrl = "")
    {
        var result = new Service()
        {
            Name = Name,
            Description = Description,
            BriefInfo = BriefInfo,
            Price = Price,
            SalePrice = SalePrice,
            Status = GlobalVariables.ActiveServiceStatusId,
            ThumbnailUrl = thumbnailUrl
        };
        return result;
    }
}