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
            builder.Property(x => x.Score).HasColumnType("decimal(5,2)").IsRequired();
            builder.HasCheckConstraint("ck_ExamAttempt_Score","Score >= 0 AND Score <= 100");
            builder.Property(x => x.AttemptNumber).IsRequired();
            builder.Property(x => x.StartedAt).HasDefaultValueSql("GetDate()").IsRequired();
            builder.Property(x => x.SubmittedAt).IsRequired(false);
            builder.Property(x=>x.Passed).HasDefaultValueSql("CAST(0 AS BIT)").IsRequired();
            builder.Property(x=>x.AutoSubmitted).HasDefaultValueSql("CAST(0 AS BIT)").IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GetDate()").IsRequired();
            builder.Property(x => x.LastUpdatedAt).IsRequired(true).HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.Exam).WithMany(x => x.ExamAttempts).HasForeignKey(x => x.ExamId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Student).WithMany(x => x.ExamAttempts).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x=>x.StudentAnswers).WithOne(x=>x.ExamAttempt).HasForeignKey(x=>x.ExamAttemptId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>  x.Grade).WithOne(x=>x.ExamAttempt).HasForeignKey<Grade>(x=>x.ExamAttemptId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}