using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Utilisateur")]
    public class Utilisateur
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nom")]
        [Required]
        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Column("Prenom")]
        [Required]
        [MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Column("Email")]
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Column("Password")]
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("Role")]
        public string Role { get; set; } = "Admin"; // "Admin" ou "AdminGenerale"

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("DateCreation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}
