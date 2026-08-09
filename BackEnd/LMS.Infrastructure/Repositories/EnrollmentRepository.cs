using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Enrollment>> GetByCourseAsync(Guid courseId) =>
        await _dbSet.AsNoTracking().Where(x => x.CourseId == courseId)
            .OrderByDescending(x => x.EnrolledAt).ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByStudentAsync(Guid studentId) =>
        await _dbSet.AsNoTracking().Where(x => x.StudentId == studentId)
            .Include(x => x.Course).OrderByDescending(x => x.EnrolledAt).ToListAsync();

    public Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId) =>
        _dbSet.AnyAsync(x => x.StudentId == studentId && x.CourseId == courseId);

    public Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId) =>
        _dbSet.SingleOrDefaultAsync(x => x.StudentId == studentId && x.CourseId == courseId);
}
