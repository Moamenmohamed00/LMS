using LMS.Domain.Entities;

namespace LMS.Application.Services.Iinfra;

/// <summary>
/// Abstracts payment provider operations (Stripe, Paymob).
/// Implemented in Infrastructure layer per provider.
/// </summary>
public interface IPaymentGateway
{
    /// <summary>Provider name (e.g. "Stripe", "Paymob").</summary>
    string ProviderName { get; }

    /// <summary>Creates a checkout session and returns the URL the client should redirect to.</summary>
    Task<string> CreateCheckoutSessionAsync(Course course, ApplicationUser student, string successUrl, string cancelUrl);

    /// <summary>Verifies webhook authenticity and returns the parsed payment result.</summary>
    Task<WebhookResult?> VerifyWebhookAsync(string payload, string signatureHeader);
}

/// <summary>Result of a verified payment webhook.</summary>
public record WebhookResult(
    string ProviderTransactionId,
    Guid StudentId,
    Guid CourseId,
    decimal Amount,
    string Currency,
    bool IsSuccess
);
