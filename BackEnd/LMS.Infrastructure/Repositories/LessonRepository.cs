using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    public LessonRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Lesson>> GetByModuleAsync(Guid moduleId) =>
        await _dbSet.AsNoTracking().Where(x => x.ModuleId == moduleId)
            .OrderBy(x => x.OrderIndex).ToListAsync();

    public Task<Lesson?> GetWithContentAsync(Guid lessonId) =>
        _dbSet.AsNoTracking()
            .Include(x => x.LessonContents)
            .Include(x => x.Quiz)
            .Include(x => x.Assignment)
            .SingleOrDefaultAsync(x => x.Id == lessonId);

    public async Task<int> GetMaxOrderIndexAsync(Guid moduleId) =>
        await _dbSet.Where(x => x.ModuleId == moduleId)
            .Select(x => (int?)x.OrderIndex).MaxAsync() ?? 0;

    public Task<int> GetTotalLessonCountByCourseAsync(Guid courseId) =>
        _dbSet.CountAsync(x => x.Module.CourseId == courseId);
}
