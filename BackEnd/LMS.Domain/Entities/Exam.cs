using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public int TimeLimitMinutes { get; set; }
        public decimal PassingGrade { get; set; }
        public int MaxAttempts { get; set; }
        public bool ShuffleQuestions { get; set; }
        public bool ShuffleChoices { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
    }
}
