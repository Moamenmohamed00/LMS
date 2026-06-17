using System;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Data.Configurations
{
    public class ApplicationUserConfiguration:IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(u=>u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u=>u.LastName).IsRequired().HasMaxLength(50);
            builder.Property(u=>u.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(u=>u.Email).IsUnique();
            builder.Property(u=>u.PhoneNumber).IsRequired().HasMaxLength(256);
            builder.HasIndex(u=>u.PhoneNumber).IsUnique();
            builder.Property(u=>u.ProfileImageUrl).IsRequired().HasMaxLength(256);
            builder.HasCheckConstraint("CHK_ProfileImageUrl", "ProfileImageUrl LIKE 'https%_.__%'");
            builder.Property(u=>u.Status).IsRequired().HasConversion<string>().HasDefaultValueSql("'Pending'");

            builder.HasMany(u=>u.Enrollments).WithOne(e=>e.Student).HasForeignKey(e=>e.StudentId);
            builder.HasMany(u=>u.CoursesTaught).WithOne(c=>c.Instructor).HasForeignKey(c=>c.InstructorId);
            builder.HasMany(u=>u.Comments).WithOne(c=>c.User).HasForeignKey(c=>c.UserId);
            builder.HasMany(u=>u.Payments).WithOne(p=>p.Student).HasForeignKey(p=>p.StudentId);
            builder.HasMany(u=>u.Certificates).WithOne(c=>c.Student).HasForeignKey(c=>c.StudentId);
            builder.HasMany(u=>u.Notifications).WithOne(n=>n.User).HasForeignKey(n=>n.UserId);
            builder.HasMany(u=>u.ExamAttempts).WithOne(e=>e.Student).HasForeignKey(e=>e.StudentId);
            builder.HasMany(u=>u.QuizAttempts).WithOne(q=>q.Student).HasForeignKey(q=>q.StudentId);
            builder.HasMany(u=>u.Submissions).WithOne(s=>s.Student).HasForeignKey(s=>s.StudentId);
            builder.HasMany(u=>u.GradesGiven).WithOne(g=>g.GradedBy).HasForeignKey(g=>g.GradedById);
            builder.HasMany(u=>u.AuditLogs).WithOne(a=>a.User).HasForeignKey(a=>a.UserId);
            builder.HasMany(u=>u.RefreshTokens).WithOne(r=>r.User).HasForeignKey(r=>r.UserId);
        }
    }
}