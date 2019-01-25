using System;
using MongoDB.Bson.Serialization.Attributes;

namespace *********.**********.BusinessServices.Models
{
    [BsonIgnoreExtraElements]
    public class Log : ICollectionName
    {
        public string EventId { get; set; }
        public int Attempt { get; set; }
        public string Proxy { get; set; }
        public TimeSpan ProxyWorkTime { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string IpAddress { get; set; }
        public bool ProxyReused { get; set; }
        //public string WorkerId { get; set; } //GAT-18 Added by Chetu
        public string EventUrl { get; set; }

        public string HttpResponseStatusCode { get; set; }

        public bool IsPulse { get; set; }

        // public string ExceptionMessage { get; set; }
        // public string InnerExceptionMessage { get; set; }
        public string CollectionName => "Log";
    }
}