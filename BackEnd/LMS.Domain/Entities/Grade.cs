using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Grade : BaseEntity
    {
        public Guid? SubmissionId { get; set; }
        public Submission? Submission { get; set; }
        public Guid? ExamAttemptId { get; set; }
        public ExamAttempt? ExamAttempt { get; set; }
        public Guid GradedById { get; set; }
        public ApplicationUser GradedBy { get; set; } = null!;
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string? Feedback { get; set; }
        public DateTime GradedAt { get; set; }
    }
}
