using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Exam>> GetByCourseAsync(Guid courseId) =>
        await _dbSet.AsNoTracking().Where(x => x.CourseId == courseId)
            .OrderBy(x => x.Title).ToListAsync();

    public Task<Exam?> GetWithQuestionsAsync(Guid examId) =>
        _dbSet.AsNoTracking().Include(x => x.Questions.OrderBy(q => q.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == examId);

    public Task<Exam?> GetWithQuestionsAndChoicesAsync(Guid examId) =>
        _dbSet.AsNoTracking().Include(x => x.Questions.OrderBy(q => q.OrderIndex))
            .ThenInclude(x => x.Choices.OrderBy(c => c.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == examId);
}
