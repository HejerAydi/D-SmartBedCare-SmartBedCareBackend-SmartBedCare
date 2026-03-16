using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Client")]
    public class Client
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("NomComplet")]
        [Required]
        [MaxLength(100)]
        public string NomComplet { get; set; } = string.Empty;

        [Column("Cin")]
        [Required]
        [MaxLength(20)]
        public string Cin { get; set; } = string.Empty;

        [Column("Tel1")]
        [Required]
        [MaxLength(20)]
        public string Tel1 { get; set; } = string.Empty;

        [Column("Tel2")]
        [MaxLength(20)]
        public string? Tel2 { get; set; }

        [Column("Adresse")]
        [MaxLength(255)]
        public string? Adresse { get; set; }

        [Column("Latitude")]
        public decimal? Latitude { get; set; }

        [Column("Longitude")]
        public decimal? Longitude { get; set; }

        [Column("PieceJointe")]
        public byte[]? PieceJointe { get; set; }

        [Column("DateCreation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}
