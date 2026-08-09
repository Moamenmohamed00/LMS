using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class QuizRepository : GenericRepository<Quiz>, IQuizRepository
{
    public QuizRepository(LMSDBContext context) : base(context) { }

    public Task<Quiz?> GetByLessonAsync(Guid lessonId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.LessonId == lessonId);

    public Task<Quiz?> GetWithQuestionsAsync(Guid quizId) =>
        _dbSet.AsNoTracking().Include(x => x.Questions.OrderBy(q => q.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == quizId);

    public Task<Quiz?> GetWithQuestionsAndChoicesAsync(Guid quizId) =>
        _dbSet.AsNoTracking().Include(x => x.Questions.OrderBy(q => q.OrderIndex))
            .ThenInclude(x => x.Choices.OrderBy(c => c.OrderIndex))
            .SingleOrDefaultAsync(x => x.Id == quizId);
}
