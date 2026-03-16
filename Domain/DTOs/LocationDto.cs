using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class LocationDTO
    {
        public int? Id { get; set; }
        public int ClientId { get; set; }
        public string? ClientNom { get; set; }
        public DateTime DateLocation { get; set; }
        public DateTime DateLivraison { get; set; }
        public DateTime DateRecuperation { get; set; }
        public string Statut { get; set; } = "EnCours";
        public decimal MontantTotal { get; set; }
        public int? CreatedBy { get; set; }
        public List<LocationLitDTO> Lits { get; set; } = new();
        public List<LocationRubriqueDTO> Rubriques { get; set; } = new();
    }

    public class CreateLocationDTO
    {
        [Required] public int ClientId { get; set; }
        [Required] public DateTime DateLivraison { get; set; }
        [Required] public DateTime DateRecuperation { get; set; }
        [Required][MinLength(1, ErrorMessage = "Au moins un lit est requis")]
        public List<int> LitIds { get; set; } = new();
        public List<LocationRubriqueCreateDTO> Rubriques { get; set; } = new();
    }

    public class UpdateLocationDTO
    {
        public DateTime? DateLivraison { get; set; }
        public DateTime? DateRecuperation { get; set; }
        [RegularExpression("^(EnCours|Terminé|Annulé)$")]
        public string? Statut { get; set; }
    }

    public class LocationLitDTO
    {
        public int LitId { get; set; }
        public string? NumeroSerie { get; set; }
        public decimal PrixLocation { get; set; }
        public decimal FraisTransport { get; set; }
    }

    public class LocationRubriqueDTO
    {
        public int RubriqueId { get; set; }
        public string? NomRubrique { get; set; }
        public string? Description { get; set; }
    }

    public class LocationRubriqueCreateDTO
    {
        public int RubriqueId { get; set; }
        public string? Description { get; set; }
    }
}
