using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ILessonRepository : IGenericRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetByModuleAsync(Guid moduleId);
    Task<Lesson?> GetWithContentAsync(Guid lessonId);
    Task<int> GetMaxOrderIndexAsync(Guid moduleId);
    Task<int> GetTotalLessonCountByCourseAsync(Guid courseId);
}
