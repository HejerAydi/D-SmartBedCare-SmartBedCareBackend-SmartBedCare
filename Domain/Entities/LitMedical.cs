using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("LitMedical")]
    public class LitMedical
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("NumeroSerie")]
        [Required]
        [MaxLength(50)]
        public string NumeroSerie { get; set; } = string.Empty;

        [Column("NombreFonction")]
        public int NombreFonction { get; set; }

        [Column("TypeLit")]
        [Required]
        public string TypeLit { get; set; } = string.Empty; // "3 Fonction" | "4 Fonction"

        [Column("TypeMatelas")]
        [Required]
        public string TypeMatelas { get; set; } = string.Empty; // "AR" | "Gauffrier" | "AxTair" | "Normale"

        [Column("PrixLocation")]
        public decimal PrixLocation { get; set; }

        [Column("FraisTransport")]
        public decimal FraisTransport { get; set; } = 0;

        [Column("ImageMatelas")]
        [MaxLength(255)]
        public string? ImageMatelas { get; set; }

        [Column("Disponible")]
        public bool Disponible { get; set; } = true;

        [Column("DateAjout")]
        public DateTime DateAjout { get; set; } = DateTime.Now;
    }
}
