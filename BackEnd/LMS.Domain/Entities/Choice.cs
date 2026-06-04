using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Choice : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
