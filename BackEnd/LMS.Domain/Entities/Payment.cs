using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public PaymentProvider Provider { get; set; }
        public string ProviderTransactionId { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
