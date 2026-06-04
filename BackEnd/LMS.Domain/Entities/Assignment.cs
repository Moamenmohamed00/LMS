using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public DateTime Deadline { get; set; }
        public decimal MaxGrade { get; set; }
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
