using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Submission : BaseEntity
    {
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public Grade? Grade { get; set; }
    }
}
