namespace LMS.Application.Services.Iinfra;

/// <summary>
/// Generates PDF certificates with QR codes.
/// Implemented in Infrastructure layer using iText7 + QRCoder.
/// </summary>
public interface ICertificateGenerator
{
    /// <summary>
    /// Generates a PDF certificate and returns the raw bytes.
    /// </summary>
    Task<byte[]> GeneratePdfAsync(
        string studentName,
        string courseName,
        string certificateNumber,
        DateTime issuedAt,
        string verificationUrl);
}
