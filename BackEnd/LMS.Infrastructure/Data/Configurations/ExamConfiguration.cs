using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x=>x.TimeLimitMinutes).IsRequired();
            builder.HasCheckConstraint("ck_Exam_TimeLimit","TimeLimitMinutes > 0");
            builder.Property(x=>x.PassingGrade).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasCheckConstraint("ck_Exam_PassingGrade","PassingGrade > 0");
            builder.Property(x=>x.MaxAttempts).IsRequired();
            builder.HasCheckConstraint("ck_Exam_MaxAttempts","MaxAttempts > 0");
            builder.Property(x=>x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x=>x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");

            // Relationships
            builder.HasOne(x=>x.Course)
            .WithMany(x=>x.Exams)
            .HasForeignKey(x=>x.CourseId);

            builder.HasMany(x=>x.Questions)
            .WithOne(x=>x.Exam)
            .HasForeignKey(x=>x.ExamId);

            builder.HasMany(x=>x.ExamAttempts)
            .WithOne(x=>x.Exam)
            .HasForeignKey(x=>x.ExamId);

        }
    }
}