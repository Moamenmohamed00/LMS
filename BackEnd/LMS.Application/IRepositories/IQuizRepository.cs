using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IQuizRepository : IGenericRepository<Quiz>
{
    Task<Quiz?> GetByLessonAsync(Guid lessonId);
    Task<Quiz?> GetWithQuestionsAsync(Guid quizId);
    Task<Quiz?> GetWithQuestionsAndChoicesAsync(Guid quizId);
}
