using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
