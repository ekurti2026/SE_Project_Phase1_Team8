using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyshareDL.Enums;
using KeyShareDL.Entities;

namespace KeyshareDL.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // FK
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
