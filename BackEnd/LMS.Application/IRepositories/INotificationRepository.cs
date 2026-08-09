using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserAsync(Guid userId);
    Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
    Task MarkAsReadAsync(Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);
}
