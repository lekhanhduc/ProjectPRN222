using System.Net;

namespace E_Learning.Servies
{
    public class FileService
    {
        private readonly string _baseDirectory;
        private readonly ILogger<FileService> _logger;

        public FileService(ILogger<FileService> logger)
        {
            // This directory can be set dynamically from configuration or environment variables
            _baseDirectory = "E:/FPT_UNIVERSITY/PRN222/"; // Modify the path to suit your needs
            _logger = logger;
        }

        // Store the file and return the file name
        public async Task<string> StoreAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // URL-encode the file name to handle special characters in file names
            string encodedFileName = WebUtility.UrlEncode(file.FileName);
            string finalName = $"{DateTime.UtcNow.Ticks}-{encodedFileName}";

            // Combine base directory, folder, and final file name to get the full file path
            string filePath = Path.Combine(_baseDirectory, folder, finalName);

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _logger.LogInformation("File Path: {FilePath}", filePath);

            // Save the file asynchronously
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }

            return finalName;
        }
    }
}
