using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class UtilisateurDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format email invalide")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Password { get; set; }

        public string Role { get; set; } = "Admin";

        public bool IsActive { get; set; } = true;

        public DateTime? DateCreation { get; set; }
    }

    public class CreateUtilisateurDTO
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MaxLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format email invalide")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "Admin";
    }

    public class UpdateUtilisateurDTO
    {
        [MaxLength(50)]
        public string? Nom { get; set; }

        [MaxLength(50)]
        public string? Prenom { get; set; }

        [EmailAddress(ErrorMessage = "Format email invalide")]
        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Role { get; set; }

        public bool? IsActive { get; set; }
    }
}
