namespace Clubee.API.Models.Base
{
    public class UploadImageModel
    {
        public readonly string ImageUrl;
        public readonly string ThumbnailUrl;

        public UploadImageModel(string imageUrl, string thumbnailUrl)
        {
            this.ImageUrl = imageUrl;
            this.ThumbnailUrl = thumbnailUrl;
        }
    }
}
