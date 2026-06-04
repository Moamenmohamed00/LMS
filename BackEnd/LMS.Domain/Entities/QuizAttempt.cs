using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class QuizAttempt : BaseEntity
    {
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
        public decimal Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}
