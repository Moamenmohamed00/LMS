using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(LMSDBContext context) : base(context) { }

    public Task<Course?> GetWithModulesAsync(Guid courseId) =>
        _dbSet.AsNoTracking()
            .Include(x => x.Modules.OrderBy(m => m.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == courseId);

    public async Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId) =>
        await _dbSet.AsNoTracking().Where(x => x.InstructorId == instructorId)
            .OrderByDescending(x => x.CreatedAt).ToListAsync();

    public async Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId) =>
        await _dbSet.AsNoTracking().Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Title).ToListAsync();

    public async Task<IEnumerable<Course>> GetPublishedAsync() =>
        await _dbSet.AsNoTracking().Where(x => x.Status == CourseStatus.Published)
            .OrderBy(x => x.Title).ToListAsync();

    public Task<Course?> GetWithFullDetailsAsync(Guid courseId) =>
        _dbSet.AsNoTracking()
            .Include(x => x.Instructor)
            .Include(x => x.Category)
            .Include(x => x.Modules.OrderBy(m => m.OrderIndex))
                .ThenInclude(x => x.Lessons.OrderBy(l => l.OrderIndex))
                    .ThenInclude(x => x.LessonContents)
            .Include(x => x.Exams)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Choices)
            .SingleOrDefaultAsync(x => x.Id == courseId);
}
