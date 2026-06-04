using System.Linq.Expressions;
using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface IExamRepository:IGenericRepository<Exam>
{
    Task<IEnumerable<Exam>> GetByCourseAsync(Guid courseId);
    Task<IEnumerable<Question>> GetWithQuestionsAsync(Guid examId);
}