using System.Linq.Expressions;

namespace LMS.Application.IRepositories;

/// <summary>
/// Generic repository providing standard CRUD operations for all entities.
/// </summary>
public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(Guid id);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
}