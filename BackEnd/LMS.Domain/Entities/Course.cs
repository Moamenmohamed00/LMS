using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Course : BaseEntity
    {
        public Guid InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public CourseStatus Status { get; set; }
        public string? RejectionReason { get; set; }
        public int TotalLessons { get; set; }
        public DateTime? PublishedAt { get; set; }
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
