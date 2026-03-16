namespace Domain.DTOs
{
    public class HistoriqueDTO
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string? Description { get; set; }
        public int? UtilisateurId { get; set; }
        public string? UtilisateurNom { get; set; }
        public DateTime DateAction { get; set; }
    }
}
