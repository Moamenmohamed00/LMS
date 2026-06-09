using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.StudentId).IsRequired();
            builder.Property(x => x.CourseId).IsRequired();
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18, 2)");
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.Status).IsRequired().HasConversion<string>();
            builder.Property(x => x.ProviderTransactionId).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Provider).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GetDate()");
            builder.HasOne(x => x.Student).WithMany(x => x.Payments).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Course).WithMany(x => x.Payments).HasForeignKey(x => x.CourseId).OnDelete(DeleteBehavior.Cascade);
            builder.HasCheckConstraint("CK_Status", "Status IN ('Pending', 'Success', 'Failed')");
            builder.HasCheckConstraint("CK_Amount", "Amount > 0");
        }
    }
}