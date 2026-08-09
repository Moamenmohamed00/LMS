using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(LMSDBContext context) : base(context) { }

    public Task<Submission?> GetByStudentAndAssignmentAsync(Guid studentId, Guid assignmentId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.StudentId == studentId && x.AssignmentId == assignmentId);

    public async Task<IEnumerable<Submission>> GetByAssignmentAsync(Guid assignmentId) =>
        await _dbSet.AsNoTracking().Where(x => x.AssignmentId == assignmentId)
            .Include(x => x.Student).Include(x => x.Grade)
            .OrderByDescending(x => x.SubmittedAt).ToListAsync();

    public Task<Submission?> GetWithGradeAsync(Guid submissionId) =>
        _dbSet.AsNoTracking().Include(x => x.Grade).SingleOrDefaultAsync(x => x.Id == submissionId);
}
