using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    Task<Certificate?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId);
    Task<Certificate?> GetByCertificateNumberAsync(string certificateNumber);
    Task<IEnumerable<Certificate>> GetByStudentAsync(Guid studentId);
}
