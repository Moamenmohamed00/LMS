using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Score).IsRequired().HasColumnType("decimal(3,2)");
            builder.HasCheckConstraint("ck_Grade_Score","Score >= 0 AND Score <= MaxScore");
            builder.Property(x=>x.MaxScore).IsRequired().HasColumnType("decimal(3,2)");
            builder.Property(x=>x.Feedback).IsRequired().HasMaxLength(1000);
            builder.Property(x=>x.GradedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x=>x.SubmissionId).IsRequired();
            builder.Property(x=>x.ExamAttemptId).IsRequired();
            builder.Property(x=>x.GradedById).IsRequired();
            builder.HasOne(x=>x.Submission).WithOne(x=>x.Grade).HasForeignKey<Grade>(x=>x.SubmissionId);
            builder.HasOne(x=>x.ExamAttempt).WithOne(x=>x.Grade).HasForeignKey<Grade>(x=>x.ExamAttemptId);
            builder.HasOne(x=>x.GradedBy).WithMany(x=>x.GradesGiven).HasForeignKey(x=>x.GradedById);
        }
    }
}