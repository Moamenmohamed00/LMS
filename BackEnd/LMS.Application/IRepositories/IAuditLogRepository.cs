using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IAuditLogRepository : IGenericRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entityType, Guid entityId);
    Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId);
    Task<IEnumerable<AuditLog>> GetRecentAsync(int count = 50);
}
