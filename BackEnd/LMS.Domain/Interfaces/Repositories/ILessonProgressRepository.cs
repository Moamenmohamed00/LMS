using System.Linq.Expressions;
using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface ILessonProgressRepository:IGenericRepository<LessonProgress>
{
    Task<LessonProgress?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
    Task<IEnumerable<LessonProgress>> GetByLessonAndEnrollmentAsync(Guid lessonId, Guid enrollmentId);
    Task<IEnumerable<LessonProgress>> GetByLessonAsync(Guid lessonId);
    Task<IEnumerable<LessonProgress>> GetByUserAsync(Guid userId);
    // Task<LessonProgress> UpsertAsync(LessonProgress progress);
    Task<int> GetCompletedCountByLessonAsync(Guid lessonId);
    Task<IEnumerable<LessonProgress>>GetByEnrollmentAsync(Guid enrollmentId);
}