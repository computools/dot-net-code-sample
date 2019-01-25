using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Enums;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Models;
using *********.**********.BusinessServices.Repositories;
using MongoDB.Driver;

namespace *********.**********.BusinessServices.Implementations
{
    public class EventService : MongoBase, IEventService
    {
        public List<Event> GetEvents()
        {
            var collection = Database.GetCollection<Event>("Event");
            var eventsCount = int.Parse(ConfigurationManager.AppSettings["eventsCount"]);
            return collection.AsQueryable().Where(x => x.EventDate > DateTime.Now).OrderBy(x => x.NextTime)
                .Take(eventsCount).ToList();
        }

        public List<Event> GetAllEvents()
        {
            var collection = Database.GetCollection<Event>("Event");

            return collection.AsQueryable().Where(x => x.EventDate > DateTime.Now).OrderBy(x => x.NextTime).ToList();
        }


        public async Task SetNextTimeEvent(string bteId, bool hasTickets)
        {
            var collection = Database.GetCollection<Event>("Event");
            var filter = Builders<Event>.Filter.Where(x => x.EventId == bteId);
            var update = Builders<Event>.Update.Set(x => x.NextTime, DateTime.UtcNow.AddHours(4))
                .Set(x => x.HasTickets, hasTickets);
            await collection.UpdateManyAsync(filter, update);
        }

        public async Task DeleteEvent(string bteId)
        {
            var collection = Database.GetCollection<Event>("Event");
            var filter = Builders<Event>.Filter.Where(x => x.EventId == bteId);
            await collection.DeleteOneAsync(filter);
        }

        public IEnumerable<Event> GetEventsInPeriodNextTime(DateTime endNextTime)
        {
            var collection = Database.GetCollection<Event>("Event");
            var eventsCount = int.Parse(ConfigurationManager.AppSettings["eventsCount"]);
            //filter for ticketmaster was added to avoid getting the event without parse item
            return collection.AsQueryable()
                .Where(x => x.EventDate > DateTime.UtcNow && x.NextTime < endNextTime ||
                            x.NextTime == null && x.EventDate > DateTime.UtcNow)
                .Where(x => x.TicketType == TicketType.TicketMaster).OrderBy(x => x.NextTime).Take(eventsCount)
                .ToList();
        }
    }
}