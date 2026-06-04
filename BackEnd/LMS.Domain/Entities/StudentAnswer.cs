using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class StudentAnswer : BaseEntity
    {
        public Guid? QuizAttemptId { get; set; }
        public QuizAttempt? QuizAttempt { get; set; }
        public Guid? ExamAttemptId { get; set; }
        public ExamAttempt? ExamAttempt { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string? WrittenAnswer { get; set; }
        public Guid? SelectedChoiceId { get; set; }
        public Choice? SelectedChoice { get; set; }
        public bool IsCorrect { get; set; }
    }
}
