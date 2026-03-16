using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Paiement")]
    public class Paiement
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LocationId")]
        public int LocationId { get; set; }

        [Column("Montant")]
        public decimal Montant { get; set; }

        [Column("DatePaiement")]
        public DateTime DatePaiement { get; set; } = DateTime.Now;

        [Column("ModePaiement")]
        [MaxLength(50)]
        public string? ModePaiement { get; set; }

        [Column("Statut")]
        public string Statut { get; set; } = "NonPayé"; // "Payé" | "NonPayé"

        [ForeignKey("LocationId")]
        public Location? Location { get; set; }
    }
}
