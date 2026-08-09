using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class QuizAttemptRepository : GenericRepository<QuizAttempt>, IQuizAttemptRepository
{
    public QuizAttemptRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<QuizAttempt>> GetByStudentAndQuizAsync(Guid studentId, Guid quizId) =>
        await _dbSet.AsNoTracking().Where(x => x.StudentId == studentId && x.QuizId == quizId)
            .OrderByDescending(x => x.StartedAt).ToListAsync();

    public Task<QuizAttempt?> GetWithAnswersAsync(Guid attemptId) =>
        _dbSet.AsNoTracking().Include(x => x.StudentAnswers).ThenInclude(x => x.Question)
            .SingleOrDefaultAsync(x => x.Id == attemptId);
}
