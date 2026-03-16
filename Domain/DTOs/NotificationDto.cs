namespace Domain.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int? LocationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateNotification { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
