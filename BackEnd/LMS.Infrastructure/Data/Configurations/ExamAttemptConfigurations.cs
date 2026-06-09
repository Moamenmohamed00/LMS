using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class ExamAttemptConfiguration :IEntityTypeConfiguration<ExamAttempt>
    {
        public void Configure(EntityTypeBuilder<ExamAttempt> builder)
        {
            builder.ToTable("ExamAttempts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.ExamId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.StudentId).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Score).HasColumnType("decimal(3,2)").IsRequired();
            builder.Property(x => x.AttemptNumber).IsRequired();
            builder.Property(x => x.StartedAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(x => x.SubmittedAt).IsRequired(false);
            builder.Property(x=>x.Passed).HasDefaultValue(false).IsRequired();
            builder.Property(x=>x.AutoSubmitted).HasDefaultValue(false).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Property(x => x.LastUpdatedAt).IsRequired(false);
            builder.HasOne(x => x.Exam).WithMany(x => x.ExamAttempts).HasForeignKey(x => x.ExamId);
            builder.HasOne(x => x.Student).WithMany(x => x.ExamAttempts).HasForeignKey(x => x.StudentId);
            builder.HasMany(x=>x.StudentAnswers).WithOne(x=>x.ExamAttempt).HasForeignKey(x=>x.ExamAttemptId);
            builder.HasOne(x=>x.Grade).WithOne(x=>x.ExamAttempt).HasForeignKey<Grade>(x=>x.ExamAttemptId);
        }
    }
}