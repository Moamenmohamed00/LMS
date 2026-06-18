using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data
{
    public class CategoryCongiguration:IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c=>c.Id);
            builder.Property(c=>c.Name).IsRequired().HasMaxLength(100);
            builder.HasIndex(c=>c.Name).IsUnique();
            builder.Property(c=>c.Description).IsRequired().HasMaxLength(1000);
            builder.Property(c=>c.ImageUrl).HasMaxLength(100);
            // builder.HasCheckConstraint("CHK_ImageUrl", "ImageUrl LIKE 'https%_.__%' ");
            builder.Property(c=>c.IsActive).IsRequired().HasDefaultValueSql("CAST(0 AS BIT)");
            builder.Property(c=>c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(c=>c.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasMany(c=>c.Courses).WithOne(c=>c.Category).HasForeignKey(c=>c.CategoryId);
        }
    }
}