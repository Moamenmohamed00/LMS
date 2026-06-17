using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.ToTable("QuizAttempts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.QuizId).IsRequired();
            builder.Property(x => x.StartedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.CompletedAt).IsRequired();
            builder.Property(x => x.Score).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.Student).WithMany(x => x.QuizAttempts).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Quiz).WithMany(x => x.QuizAttempts).HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.StudentAnswers).WithOne(x => x.QuizAttempt).HasForeignKey(x => x.QuizAttemptId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}