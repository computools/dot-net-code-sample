using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Enums;

namespace *********.**********.BusinessServices.Models
{
    [BsonIgnoreExtraElements]
    public class Proxy
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        

        public DateTime LastUsed { get; set; }
        public SiteResponse Status { get; set; }
        public bool Reused { get; set; }
    }
}
