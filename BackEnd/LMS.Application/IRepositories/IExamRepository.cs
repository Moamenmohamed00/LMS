using LMS.Domain.Entities;

namespace LMS.Application.Irepo;

public interface IExamRepository:IGenericRepository<Exam>
{
    Task<IEnumerable<Exam>> GetByCourseAsync(Guid courseId);
    Task<IEnumerable<Question>> GetWithQuestionsAsync(Guid examId);
}