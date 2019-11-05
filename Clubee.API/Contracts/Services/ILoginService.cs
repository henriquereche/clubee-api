using Clubee.API.Entities;
using Clubee.API.Models.User;

namespace Clubee.API.Contracts.Services
{
    public interface ILoginService
    {
        /// <summary>
        /// Generate access token for the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserLoginResultDTO Login(User user);

        /// <summary>
        /// Generate hash from password and salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string GeneratePasswordHash(string password, string salt);
    }
}
