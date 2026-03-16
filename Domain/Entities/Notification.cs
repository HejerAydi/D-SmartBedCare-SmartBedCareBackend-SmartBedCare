using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LocationId")]
        public int? LocationId { get; set; }

        [Column("Message")]
        [Required]
        public string Message { get; set; } = string.Empty;

        [Column("DateNotification")]
        public DateTime DateNotification { get; set; }

        [Column("Type")]
        public string Type { get; set; } = string.Empty; // "RappelRecuperation" | "RappelPaiement"

        [Column("IsRead")]
        public bool IsRead { get; set; } = false;

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
    }
}
