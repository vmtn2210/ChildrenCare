using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChildrenCare.Entities
{
    public class StaffSpecialization
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Staff")]
        public int StaffId { get; set; }
        public virtual AppUser Staff { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}
