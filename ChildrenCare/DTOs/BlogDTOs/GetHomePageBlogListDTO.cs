namespace ChildrenCare.DTOs.BlogDTOs;

public class HomePageBlogDTO
{
    public int Id { get; set; }
    public int Title { get; set; }
    public int BriefInfo { get; set; }
    public string Thumbnail { get; set; }
}
public class GetHomePageBlogListDTO
{
    public List<HomePageBlogDTO> Blogs { get; set; }
}