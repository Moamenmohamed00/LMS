using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c=>c.Id);
            builder.Property(c=>c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(c=>c.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(c=>c.IsPinned).HasDefaultValueSql("CAST(0 AS BIT)");
            builder.Property(c=>c.Content).IsRequired().HasMaxLength(250);
            builder.Property(c=>c.ParentCommentId).IsRequired();
            builder.HasOne(c=>c.User).WithMany(u=>u.Comments).HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c=>c.Lesson).WithMany(c=>c.Comments).HasForeignKey(c=>c.LessonId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c=>c.Replies).WithOne(c=>c.ParentComment).HasForeignKey(c=>c.ParentCommentId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}