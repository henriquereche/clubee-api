namespace Clubee.API.Entities
{
    public class Image
    {
        public Image(
            string uri, 
            string fileName, 
            string container
            )
        {
            this.Uri = uri;
            this.FileName = fileName;
            this.Container = container;
        }

        public string Uri { get; protected set; }
        public string FileName { get; protected set; }
        public string Container { get; protected set; }
    }
}
