using Clubee.API.Models.Base;
using Clubee.API.Models.Event;
using Clubee.API.Models.Filters;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Clubee.API.Contracts.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Find event by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EventFindDTO Find(ObjectId id);

        /// <summary>
        /// Return event list.
        /// </summary>
        /// <returns></returns>
        ListResult<EventListDTO> List(EventFilter filter);

        /// <summary>
        /// Inserts a new Event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<EventFindDTO> Insert(ObjectId establishmentId, EventInsertDTO dto);

        /// <summary>
        /// Delete existing event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        EventFindDTO Delete(ObjectId establishmentId, ObjectId id);

        /// <summary>
        /// Update existing event.
        /// </summary>
        /// <param name="establishmentId"></param>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<EventFindDTO> Update(ObjectId establishmentId, ObjectId id, EventUpdateDTO dto);
    }
}
