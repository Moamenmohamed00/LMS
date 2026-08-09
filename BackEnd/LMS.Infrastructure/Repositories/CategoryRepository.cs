using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(LMSDBContext context) : base(context) { }

    public Task<Category?> GetByNameAsync(string name) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);

    public async Task<IEnumerable<Category>> GetActiveAsync() =>
        await _dbSet.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name).ToListAsync();
}
