using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class QuizConfiguration:IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.ToTable("Quizzes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ShowResultImmediately).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.Lesson).WithOne(x => x.Quiz).HasForeignKey<Quiz>(x => x.LessonId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Questions).WithOne(x => x.Quiz).HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.QuizAttempts).WithOne(x => x.Quiz).HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}