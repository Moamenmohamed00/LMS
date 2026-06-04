using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        public Guid ModuleId { get; set; }
        public Module Module { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public ContentType ContentType { get; set; }
        public int DurationMinutes { get; set; }
        public ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
        public Quiz? Quiz { get; set; }
        public Assignment? Assignment { get; set; }
    }
}
