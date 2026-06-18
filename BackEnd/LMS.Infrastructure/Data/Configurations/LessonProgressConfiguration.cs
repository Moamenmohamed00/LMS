using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder.ToTable("LessonProgress");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.LessonId, x.EnrollmentId }).IsUnique();
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.LessonId).IsRequired();
            builder.Property(x => x.EnrollmentId).IsRequired();
            builder.Property(x => x.WatchPercentage).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsCompleted).IsRequired().HasDefaultValueSql("CAST(0 AS BIT)");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.Lesson).WithMany(x => x.LessonProgresses).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Enrollment).WithMany(x => x.LessonProgresses).HasForeignKey(x => x.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}