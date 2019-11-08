using Clubee.API.Entities;
using Clubee.API.Models.Establishment;
using Clubee.API.Models.Register;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clubee.API.Contracts.Services
{
    public interface IEstablishmentService
    {
        /// <summary>
        /// Return establishment list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<EstablishmentListDTO> List();

        /// <summary>
        /// Inserts a new establishment.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Establishment> Insert(RegisterEstablishmentDTO dto);

        /// <summary>
        /// Find establishment by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EstablishmentFindDTO Find(ObjectId id);

        /// <summary>
        /// Update existing establishment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        EstablishmentFindDTO Update(ObjectId id, EstablishmentUpdateDTO dto);
    }
}
