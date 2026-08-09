using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ILessonProgressRepository : IGenericRepository<LessonProgress>
{
    Task<LessonProgress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId);
    Task<IEnumerable<LessonProgress>> GetByEnrollmentAsync(Guid enrollmentId);
    Task<int> GetCompletedCountByEnrollmentAsync(Guid enrollmentId);
}