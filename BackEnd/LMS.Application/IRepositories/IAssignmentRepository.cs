using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IAssignmentRepository : IGenericRepository<Assignment>
{
    Task<Assignment?> GetByLessonAsync(Guid lessonId);
    Task<Assignment?> GetWithSubmissionsAsync(Guid assignmentId);
}
