using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ISubmissionRepository : IGenericRepository<Submission>
{
    Task<Submission?> GetByStudentAndAssignmentAsync(Guid studentId, Guid assignmentId);
    Task<IEnumerable<Submission>> GetByAssignmentAsync(Guid assignmentId);
    Task<Submission?> GetWithGradeAsync(Guid submissionId);
}
