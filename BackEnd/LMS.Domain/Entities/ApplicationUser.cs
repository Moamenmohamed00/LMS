using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public UserStatus Status { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Grade> GradesGiven { get; set; } = new List<Grade>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
