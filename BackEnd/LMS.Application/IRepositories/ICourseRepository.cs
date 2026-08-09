using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetWithModulesAsync(Guid courseId);
    Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId);
    Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Course>> GetPublishedAsync();
    Task<Course?> GetWithFullDetailsAsync(Guid courseId);
}