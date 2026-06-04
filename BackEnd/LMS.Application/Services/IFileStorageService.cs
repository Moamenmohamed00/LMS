using Microsoft.AspNetCore.Http;
namespace LMS.Application.Services;
public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folder);
    Task DeleteFileAsync(string fileUrl);
    Task<string> GetFileUrlAsync(string fileUrl);
}