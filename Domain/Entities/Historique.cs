using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Historique")]
    public class Historique
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("TableName")]
        [Required]
        [MaxLength(50)]
        public string TableName { get; set; } = string.Empty;

        [Column("Action")]
        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        [Column("RecordId")]
        public int RecordId { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("UtilisateurId")]
        public int? UtilisateurId { get; set; }

        [Column("DateAction")]
        public DateTime DateAction { get; set; } = DateTime.Now;

        [ForeignKey("UtilisateurId")]
        public Utilisateur? Utilisateur { get; set; }
    }
}
