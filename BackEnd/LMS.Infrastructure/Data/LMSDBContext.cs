using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Data
{
    public class LMSDBContext:IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public LMSDBContext(DbContextOptions<LMSDBContext> options):base(options)
        {}
        // public DbSet<ApplicationUser> Users {  get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Course > Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamAttempt> ExamAttempt { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonContent> LessonsContent { get; set; }
        public DbSet<LessonProgress> LessonsProgress { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(LMSDBContext).Assembly);
        }
    }
}
