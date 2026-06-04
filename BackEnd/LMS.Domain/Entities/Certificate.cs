using LMS.Domain.Common;

namespace LMS.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string CertificateNumber { get; set; } = string.Empty;
        public string PdfUrl { get; set; } = string.Empty;
        public string QrCodeData { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
    }
}
