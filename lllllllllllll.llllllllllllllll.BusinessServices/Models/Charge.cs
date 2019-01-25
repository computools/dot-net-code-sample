using MongoDB.Bson.Serialization.Attributes;

namespace *********.**********.BusinessServices.Models
{
    public class Charge
    {
        [BsonIgnore]
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
