using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace E_Learning.Servies.Impl
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadVideoChunked(Stream file, string fileName, string folderName)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, file),
                Folder = folderName
            };

            var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams, bufferSize: 6_000_000);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            throw new Exception($"Upload thất bại: {uploadResult.Error?.Message}");
        }


    }
}
