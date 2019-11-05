using Clubee.API.Contracts.Services;
using Clubee.API.Models.Base;
using ImageMagick;
using System.IO;

namespace Clubee.API.Services
{
    public class ImageService : IImageService
    {
        public const string ImageFormat = "jpeg";
        public const int ThumbnailAspectSize = 150;

        private readonly ImageOptimizer ImageOptimizer;

        public ImageService()
        {
            this.ImageOptimizer = new ImageOptimizer();
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
    }
}
