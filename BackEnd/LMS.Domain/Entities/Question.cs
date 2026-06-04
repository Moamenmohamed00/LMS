using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Question : BaseEntity
    {
        public Guid? QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public Guid? ExamId { get; set; }
        public Exam? Exam { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public string? ImageUrl { get; set; }
        public int OrderIndex { get; set; }
        public decimal Points { get; set; }
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
