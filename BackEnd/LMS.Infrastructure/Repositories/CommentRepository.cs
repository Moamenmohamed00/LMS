using LMS.Application.IRepositories;
using LMS.Domain.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public sealed class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(LMSDBContext context) : base(context) { }

    public async Task<IEnumerable<Comment>> GetByLessonAsync(Guid lessonId) =>
        await _dbSet.AsNoTracking().Where(x => x.LessonId == lessonId && x.ParentCommentId == null)
            .Include(x => x.User).Include(x => x.Replies).ThenInclude(x => x.User)
            .OrderByDescending(x => x.IsPinned).ThenByDescending(x => x.CreatedAt).ToListAsync();

    public Task<Comment?> GetWithRepliesAsync(Guid commentId) =>
        _dbSet.AsNoTracking().Include(x => x.User)
            .Include(x => x.Replies).ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == commentId);
}
