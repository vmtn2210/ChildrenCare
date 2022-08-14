using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
