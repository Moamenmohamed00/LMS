using LMS.Domain.Entities;
namespace LMS.Application.Irepo
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetWithModulesAsync(Guid CourseId);
        Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId);
        Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Course>> GetPublishedAsync();

    }
}