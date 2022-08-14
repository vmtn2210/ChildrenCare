using System;
using System.Collections.Generic;

namespace ChildrenCare.Entities
{
    public partial class Prescription
    {
        public int Id { get; set; }
        public int? CustomerAccountId { get; set; }
        public int? AuthorAccountId { get; set; }
        public string? PrescriptionContent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ReservationId { get; set; }

        public virtual AppUser? AuthorAccount { get; set; }
        public virtual AppUser? CustomerAccount { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
