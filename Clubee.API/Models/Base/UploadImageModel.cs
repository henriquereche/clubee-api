using Clubee.API.Entities;

namespace Clubee.API.Models.Base
{
    public class UploadImageModel
    {
        public readonly Image Image;
        public readonly Image Thumbnail;

        public UploadImageModel(Image image, Image thumbnail)
        {
            this.Image = image;
            this.Thumbnail = thumbnail;
        }
    }
}
