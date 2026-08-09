using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class CertificateRepository : GenericRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(LMSDBContext context) : base(context) { }

    public Task<Certificate?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.StudentId == studentId && x.CourseId == courseId);

    public Task<Certificate?> GetByCertificateNumberAsync(string certificateNumber) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.CertificateNumber == certificateNumber);

    public async Task<IEnumerable<Certificate>> GetByStudentAsync(Guid studentId) =>
        await _dbSet.AsNoTracking().Where(x => x.StudentId == studentId)
            .Include(x => x.Course).OrderByDescending(x => x.IssuedAt).ToListAsync();
}
