using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId) =>
        await _dbSet.AsNoTracking().Where(x => x.EntityType == entityType && x.EntityId == entityId.ToString())
            .OrderByDescending(x => x.Timestamp).ToListAsync();

    public async Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId) =>
        await _dbSet.AsNoTracking().Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Timestamp).ToListAsync();

    public async Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 50) =>
        await _dbSet.AsNoTracking().OrderByDescending(x => x.Timestamp)
            .Take(Math.Max(0, count)).ToListAsync();
}
