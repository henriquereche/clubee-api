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
        /// <param name="establishment"></param>
        /// <returns></returns>
        UserLoginResultDTO Login(User user, Establishment establishment);

        /// <summary>
        /// Generate access token for the specified user.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        UserLoginResultDTO Login(UserLoginDTO dto);

        /// <summary>
        /// Generate hash from password and salt.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string GeneratePasswordHash(string password, string salt);
    }
}
