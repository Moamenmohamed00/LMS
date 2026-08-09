using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(LMSDBContext context) : base(context) { }

    public Task<Assignment?> GetByLessonAsync(Guid lessonId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.LessonId == lessonId);

    public Task<Assignment?> GetWithSubmissionsAsync(Guid assignmentId) =>
        _dbSet.AsNoTracking()
            .Include(x => x.Submissions)
                .ThenInclude(x => x.Student)
            .Include(x => x.Submissions)
                .ThenInclude(x => x.Grade)
            .SingleOrDefaultAsync(x => x.Id == assignmentId);
}
