using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class LessonContentConfiguration : IEntityTypeConfiguration<LessonContent>
    {
        public void Configure(EntityTypeBuilder<LessonContent> builder)
        {
            builder.ToTable("LessonContents");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.LessonId).IsRequired();
            builder.Property(x => x.Url).IsRequired().HasMaxLength(255);
            builder.HasCheckConstraint("CK_Url", "Url LIKE 'http%://%' OR Url LIKE 'https://%'");
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
            builder.Property(x => x.FileSize).IsRequired().HasColumnType("bigint");
            builder.Property(x => x.Type).IsRequired().HasConversion<string>();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.Lesson).WithMany(x => x.LessonContents).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}