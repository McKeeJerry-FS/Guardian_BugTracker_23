using Guardian_BugTracker_23.Data.Enums;

namespace Guardian_BugTracker_23.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsynC(IFormFile? file);
        public string? ConvertByteArrayToFile(byte[]? FileData, string? extension, DefaultImage defaultImage);
    }
}
