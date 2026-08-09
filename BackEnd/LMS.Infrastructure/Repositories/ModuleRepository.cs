using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class ModuleRepository : GenericRepository<Module>, IModuleRepository
{
    public ModuleRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Module>> GetByCourseAsync(Guid courseId) =>
        await _dbSet.AsNoTracking().Where(x => x.CourseId == courseId)
            .OrderBy(x => x.OrderIndex).ToListAsync();

    public Task<Module?> GetWithLessonsAsync(Guid moduleId) =>
        _dbSet.AsNoTracking()
            .Include(x => x.Lessons.OrderBy(l => l.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == moduleId);

    public async Task<int> GetMaxOrderIndexAsync(Guid courseId) =>
        await _dbSet.Where(x => x.CourseId == courseId)
            .Select(x => (int?)x.OrderIndex).MaxAsync() ?? 0;
}
