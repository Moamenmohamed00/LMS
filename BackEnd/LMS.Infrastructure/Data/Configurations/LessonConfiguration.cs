using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.OrderIndex).IsRequired().HasColumnType("int");
            builder.Property(x=>x.ContentType).IsRequired().HasConversion<string>();
            builder.Property(x=>x.DurationMinutes).IsRequired().HasColumnType("int");
            builder.HasCheckConstraint("CK_DurationMinutes", "DurationMinutes > 0");
            builder.HasCheckConstraint("CK_OrderIndex", "OrderIndex > 0");
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.ModuleId).IsRequired();
            builder.HasOne(x => x.Module).WithMany(x => x.Lessons).HasForeignKey(x => x.ModuleId);
            builder.HasMany(x=>x.LessonContents).WithOne(x=>x.Lesson).HasForeignKey(x=>x.LessonId);
            builder.HasMany(x=>x.Comments).WithOne(x=>x.Lesson).HasForeignKey(x=>x.LessonId);
            builder.HasMany(x=>x.LessonProgresses).WithOne(x=>x.Lesson).HasForeignKey(x=>x.LessonId);
            builder.HasOne(x=>x.Quiz).WithOne(x=>x.Lesson).HasForeignKey<Quiz>(x=>x.LessonId);
            builder.HasOne(x=>x.Assignment).WithOne(x=>x.Lesson).HasForeignKey<Assignment>(x=>x.LessonId);
        }
    }
}