using Clubee.API.Contracts.Infrastructure.Storage;
using Clubee.API.Contracts.Services;
using Clubee.API.Entities;
using Clubee.API.Models.Base;
using ImageMagick;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Clubee.API.Services
{
    public class ImageService : IImageService
    {
        public const string ImageFormat = "jpeg";
        public const int ThumbnailAspectSize = 150;

        private readonly ImageOptimizer ImageOptimizer;
        private readonly IObjectStorageProvider ObjectStorageProvider;

        public ImageService(IObjectStorageProvider objectStorageProvider)
        {
            this.ImageOptimizer = new ImageOptimizer();
            this.ObjectStorageProvider = objectStorageProvider;
        }

        /// <summary>
        /// Generate compress result from base64 image.
        /// </summary>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        public CompressedImageModel CompressFromBase64(string base64Image)
        {
            MagickFormat format = MagickFormat.Jpeg;
            IMagickImage image = MagickImage.FromBase64(base64Image);

            MemoryStream compressedBuffer = new MemoryStream();
            MemoryStream thumbnailBuffer = new MemoryStream();

            image.Write(compressedBuffer, format);

            image.Resize(ImageService.ThumbnailAspectSize, ImageService.ThumbnailAspectSize);
            image.Write(thumbnailBuffer, format);

            compressedBuffer.Seek(0, SeekOrigin.Begin);
            thumbnailBuffer.Seek(0, SeekOrigin.Begin);

            this.ImageOptimizer.LosslessCompress(compressedBuffer);
            this.ImageOptimizer.LosslessCompress(thumbnailBuffer);

            return new CompressedImageModel(
                compressedBuffer.GetBuffer(),
                thumbnailBuffer.GetBuffer(),
                ImageService.ImageFormat
            );
        }

        /// <summary>
        /// Upload image to object storage.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        public async Task<UploadImageModel> UploadImage(string container, string base64Image)
        {
            CompressedImageModel compressedImage = this.CompressFromBase64(base64Image);

            string imageName = $"{this.GetRandomGuidStringValue()}.{compressedImage.Format}";
            string imageUrl = await this.ObjectStorageProvider.SetObject(
                container,
                imageName,
                compressedImage.Buffer
            );

            string thumbnailName = $"{this.GetRandomGuidStringValue()}.{compressedImage.Format}";
            string thumbnailUrl = await this.ObjectStorageProvider.SetObject(
                container,
                thumbnailName,
                compressedImage.ThumbnailBuffer
            );

            return new UploadImageModel(
                new Image(
                    imageUrl, 
                    imageName, 
                    container
                ), 
                new Image(
                    thumbnailUrl,
                    thumbnailName,
                    container
                )
            );
        }

        /// <summary>
        /// Delete existing image.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task DeleteImage(Image image)
        {
            await this.ObjectStorageProvider
                .DeleteObject(image.Container, image.FileName);
        }

        /// <summary>
        /// Generates new guid value.
        /// </summary>
        /// <returns></returns>
        private string GetRandomGuidStringValue()
            => Guid.NewGuid().ToString();
    }
}
