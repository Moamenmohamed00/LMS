using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByStudentAsync(Guid studentId);
    Task<Payment?> GetByProviderTransactionIdAsync(string providerTransactionId);
    Task<Payment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId);
}
