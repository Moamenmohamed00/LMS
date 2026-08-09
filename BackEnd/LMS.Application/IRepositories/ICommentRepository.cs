using LMS.Domain.Entities;

namespace LMS.Application.IRepositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByLessonAsync(Guid lessonId);
    Task<Comment?> GetWithRepliesAsync(Guid commentId);
}
