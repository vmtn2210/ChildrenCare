using System.ComponentModel.DataAnnotations;

namespace ChildrenCare.Entities
{
    public class AppParameter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string DisplayName { get; set; }

        public string Value { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }

    }
}
