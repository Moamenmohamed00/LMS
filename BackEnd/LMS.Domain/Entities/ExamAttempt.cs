using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class ExamAttempt : BaseEntity
    {
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
        public int AttemptNumber { get; set; }
        public decimal Score { get; set; }
        public bool Passed { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public bool AutoSubmitted { get; set; }
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public Grade? Grade { get; set; }
    }
}
