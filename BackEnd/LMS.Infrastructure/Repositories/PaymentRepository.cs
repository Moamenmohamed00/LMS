using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Payment>> GetByStudentAsync(Guid studentId) =>
        await _dbSet.AsNoTracking().Where(x => x.StudentId == studentId)
            .Include(x => x.Course).OrderByDescending(x => x.CreatedAt).ToListAsync();

    public Task<Payment?> GetByProviderTransactionIdAsync(string providerTransactionId) =>
        _dbSet.SingleOrDefaultAsync(x => x.ProviderTransactionId == providerTransactionId);

    public Task<Payment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId) =>
        _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.StudentId == studentId && x.CourseId == courseId);
}
