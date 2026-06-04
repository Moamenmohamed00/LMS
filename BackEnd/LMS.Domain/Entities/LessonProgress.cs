using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class LessonProgress : BaseEntity
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public decimal WatchPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
    }
}
