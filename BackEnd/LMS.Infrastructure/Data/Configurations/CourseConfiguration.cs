using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.ThumbnailUrl).HasMaxLength(500).IsRequired();
            builder.HasCheckConstraint("CK_Course_Price", "Price >= 0");
            builder.HasCheckConstraint("CK_Course_ThumbnailUrl", "ThumbnailUrl LIKE 'http(s)://%' OR ThumbnailUrl LIKE '%.(jpg|jpeg|png|gif)'");
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired().HasConversion<string>();
            builder.Property(x => x.TotalLessons).HasDefaultValue(0);
            builder.Property(x => x.PublishedAt).HasDefaultValueSql("GETDATE()");
            builder.HasOne(x => x.Instructor).WithMany(x => x.CoursesTaught).HasForeignKey(x => x.InstructorId);
            builder.HasOne(x => x.Category).WithMany(x => x.Courses).HasForeignKey(x => x.CategoryId);
            builder.HasMany(x => x.Modules).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
            builder.HasMany(x => x.Enrollments).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
            builder.HasMany(x => x.Exams).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
            builder.HasMany(x => x.Certificates).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
            builder.HasMany(x => x.Payments).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
            builder.HasQueryFilter(x => x.Status == CourseStatus.Published);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired(false);
        }
    }
}