namespace Clubee.API.Models.Base
{
    public class CompressedImageModel
    {
        public readonly byte[] Buffer;
        public readonly byte[] ThumbnailBuffer;
        public readonly string Format;

        public CompressedImageModel(
            byte[] buffer,
            byte[] thumbnailBuffer,
            string format
            )
        {
            this.Buffer = buffer;
            this.ThumbnailBuffer = thumbnailBuffer;
            this.Format = format;
        }
    }
}
