using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface IModuleRepository : IGenericRepository<Module>
{
    Task<IEnumerable<Module>> GetByCourseAsync(Guid courseId);
    Task<Module?> GetWithLessonsAsync(Guid moduleId);
    Task<int> GetMaxOrderIndexAsync(Guid courseId);
}
