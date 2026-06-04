using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Quiz : BaseEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public bool ShowResultImmediately { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}
