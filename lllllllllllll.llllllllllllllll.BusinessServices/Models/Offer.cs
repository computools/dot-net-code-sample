using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace *********.**********.BusinessServices.Models
{
    public class Offer
    {
        public string OfferId { get; set; }
        [BsonIgnore]
        public string ListingVersionId { get; set; }
        [BsonIgnore]
        public string ListingId { get; set; }
        public string Name { get; set; }
        public string ListPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string FaceValue { get; set; }
        public string Currency { get; set; }
        public int? PriceLevelId { get; set; }
        public List<int> SellableQuantities { get; set; }
        public List<Charge> Charges { get; set; }
        [BsonIgnore]
        public List<string> Places { get; set; }
    }
    
}
