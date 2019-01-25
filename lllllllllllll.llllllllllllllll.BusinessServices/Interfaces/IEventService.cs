using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface IEventService
    {
        List<Event> GetEvents();

        List<Event> GetAllEvents();

        IEnumerable<Event> GetEventsInPeriodNextTime(DateTime endNextTime);
        Task SetNextTimeEvent(string bteId, bool hasTickets);

        Task DeleteEvent(string bteId);
    }
}