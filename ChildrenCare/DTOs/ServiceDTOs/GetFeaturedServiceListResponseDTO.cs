namespace ChildrenCare.DTOs.ServiceDTOs;

public class FeaturedServiceResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BriefInfo { get; set; }

    public string ThumbnailUrl { get; set; }
}

public class GetFeaturedServiceListResponseDTO
{
    public GetFeaturedServiceListResponseDTO(IEnumerable<FeaturedServiceResponseDTO> services)
    {
        Services = services;
    }

    public IEnumerable<FeaturedServiceResponseDTO> Services { get; set; }
}