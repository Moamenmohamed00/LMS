using System.Linq.Expressions;
using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces.Repositories;

public interface ICourseRepository:IGenericRepository<Course>
{
    Task<IEnumerable<Course>> GetWithModulesAsync(Guid CourseId);
    Task<IEnumerable<Course>> GetByInstructorAsync(Guid instructorId);
    Task<IEnumerable<Course>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Course>> GetPublishedAsync();

}
