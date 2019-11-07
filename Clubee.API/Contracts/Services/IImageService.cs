using Clubee.API.Models.Base;
using System.Threading.Tasks;

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

        /// <summary>
        /// Upload image to object storage.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="base64Image"></param>
        /// <returns></returns>
        Task<UploadImageModel> UploadImage(string container, string base64Image);
    }
}
