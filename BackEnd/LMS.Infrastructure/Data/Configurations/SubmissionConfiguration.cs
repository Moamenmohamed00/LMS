using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.SubmittedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            // builder.Property(x => x.Grade).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.FileUrl).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Assignment).WithMany(x => x.Submissions).HasForeignKey(x => x.AssignmentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Student).WithMany(x => x.Submissions).HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Grade).WithOne(x => x.Submission).HasForeignKey<Grade>(x => x.SubmissionId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}