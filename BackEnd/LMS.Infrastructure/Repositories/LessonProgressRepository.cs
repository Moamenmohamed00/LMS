using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class LessonProgressRepository : GenericRepository<LessonProgress>, ILessonProgressRepository
{
    public LessonProgressRepository(LMSDBContext context) : base(context) { }

    public Task<LessonProgress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId) =>
        _dbSet.SingleOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.LessonId == lessonId);

    public async Task<IEnumerable<LessonProgress>> GetByEnrollmentAsync(Guid enrollmentId) =>
        await _dbSet.AsNoTracking().Where(x => x.EnrollmentId == enrollmentId)
            .OrderBy(x => x.Lesson.OrderIndex).ToListAsync();

    public Task<int> GetCompletedCountByEnrollmentAsync(Guid enrollmentId) =>
        _dbSet.CountAsync(x => x.EnrollmentId == enrollmentId && x.IsCompleted);
}
