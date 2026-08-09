using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IQuizAttemptRepository : IGenericRepository<QuizAttempt>
{
    Task<IEnumerable<QuizAttempt>> GetByStudentAndQuizAsync(Guid studentId, Guid quizId);
    Task<QuizAttempt?> GetWithAnswersAsync(Guid attemptId);
}
