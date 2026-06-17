using LMS.Domain.Entities;

namespace LMS.Application.Irepo
{
public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetByCourseAsync(Guid courseId);
    Task<IEnumerable<Enrollment>> GetByStudentAsync(Guid studentId);
    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId);
}
}