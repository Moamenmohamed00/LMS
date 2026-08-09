using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IGradeRepository : IGenericRepository<Grade>
{
    Task<Grade?> GetBySubmissionAsync(Guid submissionId);
    Task<Grade?> GetByExamAttemptAsync(Guid examAttemptId);
    Task<IEnumerable<Grade>> GetByGraderAsync(Guid gradedById);
}
