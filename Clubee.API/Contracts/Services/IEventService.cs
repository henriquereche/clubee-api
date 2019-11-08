﻿using Clubee.API.Models.Event;
using MongoDB.Bson;
using System.Collections.Generic;
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
        IEnumerable<EventListDTO> List();

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
    }
}
