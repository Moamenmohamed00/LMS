using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class GradeRepository : GenericRepository<Grade>, IGradeRepository
{
    public GradeRepository(LMSDBContext context) : base(context) { }

    public Task<Grade?> GetBySubmissionAsync(Guid submissionId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.SubmissionId == submissionId);

    public Task<Grade?> GetByExamAttemptAsync(Guid examAttemptId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.ExamAttemptId == examAttemptId);

    public async Task<IEnumerable<Grade>> GetByGraderAsync(Guid gradedById) =>
        await _dbSet.AsNoTracking().Where(x => x.GradedById == gradedById)
            .OrderByDescending(x => x.GradedAt).ToListAsync();
}
