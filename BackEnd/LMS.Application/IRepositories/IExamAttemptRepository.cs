using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IExamAttemptRepository : IGenericRepository<ExamAttempt>
{
    Task<IEnumerable<ExamAttempt>> GetByStudentAndExamAsync(Guid studentId, Guid examId);
    Task<ExamAttempt?> GetWithAnswersAsync(Guid attemptId);
    Task<int> GetAttemptCountAsync(Guid studentId, Guid examId);
    Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, Guid examId);
}
