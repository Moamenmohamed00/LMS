using System;
using System.Collections.Generic;
using System.Text;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a=>a.Id);
            builder.Property(a=>a.Title).IsRequired().HasMaxLength(50);
            builder.Property(a=>a.Instructions).IsRequired().HasMaxLength(1000);
            builder.Property(a=>a.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(a=>a.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(a=>a.Deadline).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(a=>a.MaxGrade).HasColumnType("decimal(18,2)").IsRequired().HasDefaultValueSql("100");
            builder.Property(a=>a.AttachmentUrl).HasMaxLength(256);
            builder.Property(a=>a.LessonId).IsRequired();
            builder.HasCheckConstraint("CHK_MaxGrade", "MaxGrade > 0");
            builder.HasCheckConstraint("CHK_Deadline", "Deadline > GETDATE()");
            builder.HasCheckConstraint("CHK_AttachmentUrl", "AttachmentUrl LIKE 'https%_.__%'");

            builder.HasMany(a=>a.Submissions).WithOne(s=>s.Assignment).HasForeignKey(s=>s.AssignmentId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
