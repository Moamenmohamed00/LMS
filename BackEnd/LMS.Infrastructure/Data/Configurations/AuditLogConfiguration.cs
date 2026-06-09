using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data
{
    public class AuditLogCongiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(a=>a.Id);
            builder.Property(a=>a.UserId).IsRequired();
            builder.Property(a=>a.Action).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.EntityType).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.EntityId).IsRequired();
            builder.Property(a=>a.NewValues).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.OldValues).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.Timestamp).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.HasCheckConstraint("CHK_Action", "Action IN ('Create','Update','Delete')");
            builder.HasCheckConstraint("CHK_TimeStamp","Timestamp > GETDATE()");
            builder.HasOne(a=>a.User).WithMany(u=>u.AuditLogs).HasForeignKey(a=>a.UserId);
        }
    }
}