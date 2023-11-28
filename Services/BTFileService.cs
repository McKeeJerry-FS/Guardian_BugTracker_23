using Guardian_BugTracker_23.Data.Enums;
using Guardian_BugTracker_23.Services.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace Guardian_BugTracker_23.Services
{
    public class BTFileService : IBTFileService
    {
        #region Properties
        private readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        private readonly string _defaultBTUserImageSrc = "/img/DefaultUserImage.png";
        private readonly string _defaultCompanyImageSrc = "/img/DefaultCompanyImage.png";
        private readonly string _defaultProjectImageSrc = "/img/DefaultProjectImage.png";
        #endregion

        public string ConvertByteArrayToFile(byte[] fileData, string extension, DefaultImage fileType)
        {
            if ((fileData == null || fileData.Length == 0))
            {
                switch (fileType)
                {
                    // BTUser Image based on the 'DefaultImage' Enum
                    case DefaultImage.BTUserImage: return _defaultBTUserImageSrc;
                    // Company Image based on the 'DefaultImage' Enum
                    case DefaultImage.CompanyImage: return _defaultCompanyImageSrc;
                    // Project Image based on the 'DefaultImage' Enum
                    case DefaultImage.ProjectImage: return _defaultProjectImageSrc;
                }
            }

            try
            {
                string fileBase64Data = Convert.ToBase64String(fileData!);
                return string.Format($"data:{extension};base64,{fileBase64Data}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            try
            {
                MemoryStream memoryStream = new();
                await file.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();
                memoryStream.Dispose();

                return byteFile;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal fileSize = bytes;
            while (Math.Round(fileSize / 1024) >= 1)
            {
                fileSize /= bytes;
                counter++;
            }
            return string.Format("{0:n1}{1}", fileSize, suffixes[counter]);
        }

        public string GetFileIcon(string file)
        {
            string fileImage = "default";

            if (!string.IsNullOrWhiteSpace(file))
            {
                fileImage = Path.GetExtension(file).Replace(".", "");
                return $"/img/content-type/{fileImage}.png";
            }
            return fileImage;
        }
    }
}
