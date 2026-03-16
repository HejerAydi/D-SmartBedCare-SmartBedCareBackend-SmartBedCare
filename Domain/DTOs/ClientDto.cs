using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class ClientDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Le nom complet est obligatoire")]
        [MaxLength(100)]
        public string NomComplet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le CIN est obligatoire")]
        [MaxLength(20)]
        public string Cin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est obligatoire")]
        [MaxLength(20)]
        public string Tel1 { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Tel2 { get; set; }

        [MaxLength(255)]
        public string? Adresse { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? PieceJointeBase64 { get; set; }
        public DateTime? DateCreation { get; set; }
    }

    public class CreateClientDTO
    {
        [Required(ErrorMessage = "Le nom complet est obligatoire")]
        [MaxLength(100)]
        public string NomComplet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le CIN est obligatoire")]
        [MaxLength(20)]
        public string Cin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est obligatoire")]
        [MaxLength(20)]
        public string Tel1 { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Tel2 { get; set; }

        [MaxLength(255)]
        public string? Adresse { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // Base64 pour upload fichier
        public string? PieceJointeBase64 { get; set; }
    }

    public class UpdateClientDTO
    {
        [MaxLength(100)]
        public string? NomComplet { get; set; }

        [MaxLength(20)]
        public string? Cin { get; set; }

        [MaxLength(20)]
        public string? Tel1 { get; set; }

        [MaxLength(20)]
        public string? Tel2 { get; set; }

        [MaxLength(255)]
        public string? Adresse { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? PieceJointeBase64 { get; set; }
    }
}
