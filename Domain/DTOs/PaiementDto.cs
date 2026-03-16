using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class PaiementDTO
    {
        public int? Id { get; set; }
        public int LocationId { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; }
        public string? ModePaiement { get; set; }
        public string Statut { get; set; } = "NonPayé";
    }

    public class CreatePaiementDTO
    {
        [Required] public int LocationId { get; set; }
        [Required][Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0")]
        public decimal Montant { get; set; }
        public string? ModePaiement { get; set; }
        [RegularExpression("^(Payé|NonPayé)$")]
        public string Statut { get; set; } = "NonPayé";
    }
}
