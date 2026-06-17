using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CourseId).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.OrderIndex).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.HasCheckConstraint("CK_OrderIndex", "OrderIndex > 0");
            builder.HasOne(x => x.Course).WithMany(x => x.Modules).HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Lessons).WithOne(x => x.Module).HasForeignKey(x => x.ModuleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}