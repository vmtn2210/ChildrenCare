using System.Collections.ObjectModel;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.DTOs.ServiceDTOs;

public class AdvancedGetServiceRequestDTO : PaginationParams
{
    public AdvancedGetServiceRequestDTO()
    {
        MaxPrice = -1;
        MinPrice = -1;
        MaxSalePrice = -1;
        MinSalePrice = -1;
    }

    public static ReadOnlyCollection<string> OrderingParams { get; } = new ReadOnlyCollection<string>(
        new string[] {
            "id_asc", "id_desc",
            "name_asc", "name_desc",
            "status_asc", "status_desc",
            "price_asc", "price_desc",
            "saleprice_asc", "saleprice_desc",
            "createddate_asc","createddate_desc",
            "lastupdate_asc","lastupdate_desc",
        }
    );

    public IEnumerable<int> FilterIds { get; set; }
    public string FilterName { get; set; }
    public IEnumerable<int> FilterStatuses { get; set; }
    public long MaxPrice { get; set; }
    public long MinPrice { get; set; }
    public long MaxSalePrice { get; set; }
    public long MinSalePrice { get; set; }
    public string MinCreatedDate { get; set; }
    public string MaxCreatedDate { get; set; }
    public string MinLastUpdate { get; set; }
    public string MaxLastUpdate { get; set; }
    public string SortBy { get; set; }

    public string Title { get; set; }
}