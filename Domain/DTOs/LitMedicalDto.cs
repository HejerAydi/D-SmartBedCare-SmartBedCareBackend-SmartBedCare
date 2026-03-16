using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class LitMedicalDTO
    {
        public int? Id { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
        public int NombreFonction { get; set; }
        public string TypeLit { get; set; } = string.Empty;
        public string TypeMatelas { get; set; } = string.Empty;
        public decimal PrixLocation { get; set; }
        public decimal FraisTransport { get; set; }
        public string? ImageMatelas { get; set; }
        public bool Disponible { get; set; }
        public DateTime? DateAjout { get; set; }
    }

    public class CreateLitMedicalDTO
    {
        [Required] [MaxLength(50)] public string NumeroSerie { get; set; } = string.Empty;
        [Required] public int NombreFonction { get; set; }
        [Required][RegularExpression("^(3 Fonction|4 Fonction)$")] public string TypeLit { get; set; } = string.Empty;
        [Required][RegularExpression("^(AR|Gauffrier|AxTair|Normale)$")] public string TypeMatelas { get; set; } = string.Empty;
        [Required] public decimal PrixLocation { get; set; }
        public decimal FraisTransport { get; set; } = 0;
        public string? ImageMatelas { get; set; }
    }

    public class UpdateLitMedicalDTO
    {
        public string? NumeroSerie { get; set; }
        public int? NombreFonction { get; set; }
        public string? TypeLit { get; set; }
        public string? TypeMatelas { get; set; }
        public decimal? PrixLocation { get; set; }
        public decimal? FraisTransport { get; set; }
        public string? ImageMatelas { get; set; }
        public bool? Disponible { get; set; }
    }
}
