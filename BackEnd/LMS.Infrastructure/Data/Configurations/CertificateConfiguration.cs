using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations;
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.StudentId).IsRequired();
        builder.Property(c => c.CourseId).IsRequired();
        builder.Property(c=>c.CertificateNumber).IsRequired();
        builder.HasIndex(c=>c.CertificateNumber).IsUnique();
        builder.Property(c=>c.QrCodeData).IsRequired();
        builder.Property(c => c.IssuedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(c => c.PdfUrl).IsRequired().HasMaxLength(256);
         builder.HasCheckConstraint("CHK_CertificateUrl", "CertificateUrl LIKE 'https%_.__%' ");
        builder.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(c => c.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        builder.HasOne(c => c.Student).WithMany(s => s.Certificates).HasForeignKey(c => c.StudentId);
        builder.HasOne(c => c.Course).WithMany(c => c.Certificates).HasForeignKey(c => c.CourseId);
    }
}
}