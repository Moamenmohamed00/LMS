using LMS.Application.IRepositories;
using LMS.Application.Services.Auth;
using LMS.Application.Services.Email;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LMS.Application.Settings;

namespace LMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SmtpSettings>()
                .Bind(configuration.GetSection(SmtpSettings.SectionName))
                .ValidateDataAnnotations().ValidateOnStart();
            services.AddOptions<AppUrlSettings>()
                .Bind(configuration.GetSection(AppUrlSettings.SectionName))
                .ValidateDataAnnotations().ValidateOnStart();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IExamAttemptRepository, ExamAttemptRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;
        }
    }
}
