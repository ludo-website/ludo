using Microsoft.AspNetCore.Http;
using Ludo.Infrastructure.DataTransferObjects;
using Ludo.Infrastructure.Responses;

namespace Ludo.Infrastructure.Repositories.Interfaces;

public interface IFileRepository
{
    public ServiceResponse<FileRecord> GetFile(string filePath, string? replacedFileName = null);
    public ServiceResponse<FileRecord> SaveFileAndGet(IFormFile file, string directoryPath);
    public ServiceResponse<string> SaveFile(IFormFile file, string directoryPath);
    public ServiceResponse<FileRecord> SaveFileAndGet(byte[] file, string directoryPath, string fileExtension);
    public ServiceResponse<string> SaveFile(byte[] file, string directoryPath, string fileExtension);
    public ServiceResponse<FileRecord> UpdateFileAndGet(IFormFile file, string filePath);
    public ServiceResponse<string> UpdateFile(IFormFile file, string filePath);
    public ServiceResponse<FileRecord> UpdateFileAndGet(byte[] file, string filePath);
    public ServiceResponse<string> UpdateFile(byte[] file, string filePath);
    public ServiceResponse DeleteFile(string filePath);
}
