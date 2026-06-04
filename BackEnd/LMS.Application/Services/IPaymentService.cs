using LMS.Domain.Entities;
namespace LMS.Application.Services;
public interface IPaymentService
{
Task<bool> VerifyWebhookAsync(string payload, string signatureHeader);
Task<string> CreateCheckoutSessionAsync(Guid cartId);
}