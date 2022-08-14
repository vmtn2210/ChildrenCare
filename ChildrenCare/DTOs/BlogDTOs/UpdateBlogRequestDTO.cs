using System.ComponentModel.DataAnnotations;
using System.Net;
using ChildrenCare.Utilities;

namespace ChildrenCare.DTOs.BlogDTOs;

//TODO: Change blog Status
public class UpdateBlogRequestDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Please enter title")]
    [MaxLength(255)]
    public string Tittle { get; set; }
    
    [Required]
    public int Status { get; set; }

    [Required(ErrorMessage = "Please enter brief infomation")]
    [MaxLength(255)]
    public string BriefInfo { get; set; }

    [Required(ErrorMessage = "Please enter description")]
    public string BlogBody { get; set; }

    public IFormFile ThumbnailFile { get; set; }

    public Entities.Blog MapAndUpdateArticle(Entities.Blog blog, string? thumbnailUrl = null)
    {
        var hasChanged = false;

        if (!string.IsNullOrWhiteSpace(Tittle) && Tittle != blog.Tittle)
        {
            blog.Tittle = Tittle;
            hasChanged = true;
        }

        
        if (Status != blog.Status)
        {
            blog.Status = Status;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(BriefInfo))
        {
            blog.BriefInfo = BriefInfo;
            hasChanged = true;
        }

        if (!string.IsNullOrWhiteSpace(BlogBody))
        {
            blog.BlogBody = BlogBody;
            hasChanged = true;
        }

        if (thumbnailUrl != null)
        {
            blog.ThumbnailUrl = thumbnailUrl;
            hasChanged = true;
        }

        if (hasChanged)
        {
            blog.LastUpdate = DateTime.Now;
        }
        else
        {
            //TODO: Handle Exception
        }

        return blog;
    }
}