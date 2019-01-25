namespace *********.**********.BusinessServices.Models
{
    using System;

    using *********.**********.BusinessServices.Enums;

    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Event 
    {
        public string EventId { get; set; }

        public string Url { get; set; }

        public DateTime? NextTime { get; set; }

        public TicketType TicketType { get; set; }

        public bool HasTickets { get; set; }

        public DateTime EventDate { get; set; }

        [BsonIgnore]
        public string TicketMasterId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EventId))
                {
                    return this.EventId;
                }
                try
                {
                    var ticketMasterUrlParams = Url.Split(new[] { "event/" }, StringSplitOptions.None);
                    return ticketMasterUrlParams[1].Contains("?") ? ticketMasterUrlParams[1].Split('?')[0] : ticketMasterUrlParams[1];
                }
                catch (Exception e)
                {
                    return string.Empty;
                }
            }

        }
    }
}
