using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Data.Configurations
{
    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> builder)
    {
        builder.Property(c=>c.Id).IsRequired();
        builder.Property(c=>c.QuestionId).IsRequired();
        builder.Property(c=>c.Text).IsRequired().HasMaxLength(150);
        builder.Property(c=>c.IsCorrect).IsRequired().HasDefaultValueSql("False");
        builder.Property(c=>c.OrderIndex).IsRequired();
        builder.Property(c=>c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(c=>c.LastUpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        builder.HasOne(c=>c.Question).WithMany(q=>q.Choices).HasForeignKey(c=>c.QuestionId);
        builder.HasMany(c=>c.StudentAnswers).WithOne(s=>s.SelectedChoice).HasForeignKey(s=>s.SelectedChoiceId);
    }
}
}