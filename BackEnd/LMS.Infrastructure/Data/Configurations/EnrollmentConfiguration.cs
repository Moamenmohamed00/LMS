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
            builder.Property(x => x.IsCompleted).HasDefaultValueSql("CAST(0 AS BIT)").IsRequired();
            builder.Property(x => x.EnrolledAt).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(x => x.CompletedAt).IsRequired(false);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.Property(x => x.LastUpdatedAt).IsRequired(true).HasDefaultValueSql("GETDATE()");
            builder.HasIndex(x=>new {x.StudentId,x.CourseId}).IsUnique();
            builder.HasOne(x => x.Student).WithMany(x => x.Enrollments).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Course).WithMany(x => x.Enrollments).HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.LessonProgresses).WithOne(x => x.Enrollment).HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}