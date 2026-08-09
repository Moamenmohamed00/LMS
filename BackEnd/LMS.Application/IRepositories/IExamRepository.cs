using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IExamRepository : IGenericRepository<Exam>
{
    Task<IEnumerable<Exam>> GetByCourseAsync(Guid courseId);
    Task<Exam?> GetWithQuestionsAsync(Guid examId);
    Task<Exam?> GetWithQuestionsAndChoicesAsync(Guid examId);
}