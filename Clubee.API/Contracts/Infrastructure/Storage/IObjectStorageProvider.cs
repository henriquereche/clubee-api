using System.Threading.Tasks;

namespace Clubee.API.Contracts.Infrastructure.Storage
{
    public interface IObjectStorageProvider
    {
        /// <summary>
        /// Inserts a new object.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        Task SetObject(string container, string objectName, byte[] buffer);

        /// <summary>
        /// Delete existing object.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Task DeleteObject(string container, string objectName);
    }
}
