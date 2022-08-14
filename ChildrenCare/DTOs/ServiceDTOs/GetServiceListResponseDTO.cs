using System.ComponentModel.DataAnnotations;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.ServiceDTOs;

public class GetServiceListResponseDTO : PaginationParams
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BriefInfo { get; set; }
    public long Price { get; set; }

    public long? SalePrice { get; set; }
    public int Status { get; set; }

    public string ThumbnailUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdated { get; set; }
}