using System.ClientModel.Primitives;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.ToTable("Enrollments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.StudentId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CourseId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ProgressPercentage).HasDefaultValue(0).HasColumnType("decimal(3,2)").IsRequired();
            builder.Property(x => x.IsCompleted).HasDefaultValue(false).IsRequired();
            builder.Property(x => x.EnrolledAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(x => x.CompletedAt).IsRequired(false);
            builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(x => x.LastUpdatedAt).IsRequired(false);
            builder.HasOne(x => x.Student).WithMany(x => x.Enrollments).HasForeignKey(x => x.StudentId);
            builder.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId);
            builder.HasMany(x => x.LessonProgresses).WithOne(x => x.Enrollment).HasForeignKey(x => x.EnrollmentId);
        }
    }
}