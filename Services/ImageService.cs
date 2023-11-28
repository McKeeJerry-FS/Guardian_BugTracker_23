using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Services.Interfaces;

namespace Guardian_BugTracker_23.Services
{
    public class ImageService : IImageService
    {

        private readonly string? _defaultBlogPostImage = "/img/img_placeholder.jpg";
        private readonly string? _defaultBlogUserImage = "/img/silo_img.jpg";
        private readonly string? _defaultCategoryImage = "/img/category_default.png";
        private readonly string? _defaultAuthorImage = "/img/headshot.png";

        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension, DefaultImage defaultImage)
        {
            try
            {
                if (fileData == null || fileData.Length == 0)
                {
                    // show default
                    switch (defaultImage)
                    {
                        case DefaultImage.BTUserImage:
                            return _defaultAuthorImage;
                        case DefaultImage.ProjectImage:
                            return _defaultBlogPostImage;
                        case DefaultImage.CompanyImage:
                            return _defaultCategoryImage;
                        
                    }
                }
                string? imageBase64Data = Convert.ToBase64String(fileData!);
                imageBase64Data = string.Format($"data:{extension};base64, {imageBase64Data}");
                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsynC(IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    byte[] byteFile = memoryStream.ToArray();
                    memoryStream.Close();

                    return byteFile;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }   
}
