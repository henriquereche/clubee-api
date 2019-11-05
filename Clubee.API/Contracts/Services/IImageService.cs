using Clubee.API.Models.Base;

namespace Clubee.API.Contracts.Services
{
    public interface IImageService
    {
        /// <summary>
        /// Generate compress result from base64 image.
        /// </summary>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        CompressedImageModel CompressFromBase64(string base64Image);
    }
}
