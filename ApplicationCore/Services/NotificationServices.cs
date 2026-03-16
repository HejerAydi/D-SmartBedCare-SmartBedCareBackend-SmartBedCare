using Domain.Entities;
using Infrastructure.Infrastructure;

namespace ApplicationCore.Services
{
    public class NotificationServices : INotificationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<Notification>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Notification>().GetAllAsyncwithfilter(
                    orderBy: q => q.OrderByDescending(n => n.DateNotification));
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<IReadOnlyList<Notification>> GetUnreadAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Notification>().GetAllAsyncwithfilter(
                    filter: n => !n.IsRead,
                    orderBy: q => q.OrderByDescending(n => n.DateNotification));
                return result.ToList();
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            try
            {
                var notif = await _unitOfWork.Repository<Notification>().GetByIdAsync(id);
                if (notif == null) throw new Exception($"Notification ID {id} introuvable.");
                notif.IsRead = true;
                await _unitOfWork.Repository<Notification>().UpdateAsync(notif);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }

        public async Task<bool> MarkAllAsReadAsync()
        {
            try
            {
                var result = await _unitOfWork.Repository<Notification>().GetAllAsyncwithfilter(n => !n.IsRead);
                var unread = result.ToList();
                foreach (var n in unread) { n.IsRead = true; await _unitOfWork.Repository<Notification>().UpdateAsync(n); }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex) { throw genException.GenericException.GenException(ex, _unitOfWork); }
        }
    }
}
