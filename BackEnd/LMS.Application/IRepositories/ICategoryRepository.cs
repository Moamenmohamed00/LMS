using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<IEnumerable<Category>> GetActiveAsync();
}
