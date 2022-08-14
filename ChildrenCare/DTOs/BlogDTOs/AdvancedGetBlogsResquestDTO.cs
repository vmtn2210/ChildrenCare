using System.Collections.ObjectModel;

namespace ChildrenCare.DTOs.BlogDTOs;

public class AdvancedGetBlogsResquestDTO
{
    public static ReadOnlyCollection<string> OrderingParams { get; } = new ReadOnlyCollection<string>(
        new string[] {
            "id_asc", "id_desc",
            "authoruserid_asc", "authoruserid_desc",
            "authorname_asc", "authorname_desc",
            "tittle_asc", "tittle_desc",
            "status_asc", "status_desc",
            "createddate_asc","createddate_desc",
            "lastupdate_asc","lastupdate_desc",
        }
    );

    public IEnumerable<int> FilterIds { get; set; }
    public IEnumerable<int> AuthorUserIds { get; set; }
    public string FilterTittle { get; set; }
    public IEnumerable<int> FilterStatuses { get; set; }
    public string MinCreatedDate { get; set; }
    public string MaxCreatedDate { get; set; }
    public string MinLastUpdate { get; set; }
    public string MaxLastUpdate { get; set; }
    public string SortBy { get; set; }
}