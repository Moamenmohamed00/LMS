using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetByUserAsync(Guid userId) =>
        await _dbSet.AsNoTracking().Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId) =>
        await _dbSet.AsNoTracking().Where(x => x.UserId == userId && !x.IsRead)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public Task<int> GetUnreadCountAsync(Guid userId) =>
        _dbSet.CountAsync(x => x.UserId == userId && !x.IsRead);

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        await _dbSet.Where(x => x.Id == notificationId).ExecuteUpdateAsync(x =>
            x.SetProperty(n => n.IsRead, true));
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        await _dbSet.Where(x => x.UserId == userId && !x.IsRead).ExecuteUpdateAsync(x =>
            x.SetProperty(n => n.IsRead, true));
    }
}
