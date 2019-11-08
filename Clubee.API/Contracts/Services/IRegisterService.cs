using Clubee.API.Models.Register;
using Clubee.API.Models.User;
using System.Threading.Tasks;

namespace Clubee.API.Contracts.Services
{
    public interface IRegisterService
    {
        /// <summary>
        /// Register a new user and establishment.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UserLoginResultDTO> Register(RegisterEstablishmentDTO dto);
    }
}
