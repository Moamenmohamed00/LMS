using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.QuizId).IsRequired(false);
            builder.Property(x=>x.ExamId).IsRequired(false);
            builder.Property(x => x.Text).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(x => x.Type).IsRequired().HasConversion<string>();
            builder.Property(x => x.OrderIndex).IsRequired();
            builder.Property(x => x.Points).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.Quiz).WithMany(x => x.Questions).HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Exam).WithMany(x=>x.Questions).HasForeignKey(x=>x.ExamId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x=>x.Choices).WithOne(x=>x.Question).HasForeignKey(x=>x.QuestionId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x=>x.StudentAnswers).WithOne(x=>x.Question).HasForeignKey(x=>x.QuestionId).OnDelete(DeleteBehavior.Restrict);
            // builder.HasCheckConstraint("CK_QuestionType", "Type IN ('MCQ', 'TrueFalse', 'WrittenAnswer')");
            builder.HasCheckConstraint("CK_OrderIndex", "OrderIndex > 0");
        }
    }
}