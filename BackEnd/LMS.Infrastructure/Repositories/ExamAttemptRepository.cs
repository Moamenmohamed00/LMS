using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class ExamAttemptRepository : GenericRepository<ExamAttempt>, IExamAttemptRepository
{
    public ExamAttemptRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<ExamAttempt>> GetByStudentAndExamAsync(Guid studentId, Guid examId) =>
        await _dbSet.AsNoTracking().Where(x => x.StudentId == studentId && x.ExamId == examId)
            .OrderByDescending(x => x.AttemptNumber).ToListAsync();

    public Task<ExamAttempt?> GetWithAnswersAsync(Guid attemptId) =>
        _dbSet.AsNoTracking().Include(x => x.StudentAnswers).ThenInclude(x => x.Question)
            .SingleOrDefaultAsync(x => x.Id == attemptId);

    public Task<int> GetAttemptCountAsync(Guid studentId, Guid examId) =>
        _dbSet.CountAsync(x => x.StudentId == studentId && x.ExamId == examId);

    public Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, Guid examId) =>
        _dbSet.SingleOrDefaultAsync(x => x.StudentId == studentId && x.ExamId == examId && x.SubmittedAt == null);
}
