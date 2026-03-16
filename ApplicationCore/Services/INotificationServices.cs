using Domain.Entities;

namespace ApplicationCore.Services
{
    public interface INotificationServices
    {
        Task<IReadOnlyList<Notification>> GetAllAsync();
        Task<IReadOnlyList<Notification>> GetUnreadAsync();
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAllAsReadAsync();
    }
}
