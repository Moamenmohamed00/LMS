using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.WrittenAnswer).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.IsCorrect).IsRequired().HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Question).WithMany(x => x.StudentAnswers).HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.SelectedChoice).WithMany(x => x.StudentAnswers).HasForeignKey(x => x.SelectedChoiceId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.ExamAttempt).WithMany(x => x.StudentAnswers).HasForeignKey(x => x.ExamAttemptId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.QuizAttempt).WithMany(x => x.StudentAnswers).HasForeignKey(x => x.QuizAttemptId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}