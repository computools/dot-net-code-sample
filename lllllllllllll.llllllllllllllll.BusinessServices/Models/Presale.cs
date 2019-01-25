using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace *********.**********.BusinessServices.Models
{
    public class Presale : ICollectionName
    {
        public int BteId { get; set; }
        public string Label { get; set; }
        [BsonIgnore]
        public string Value { get; set; }
        [BsonIgnore]
        public string Format { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonRequired]
        public DateTime? StartDate
        {
            get
            {
                DateTime result;
                bool success;
                switch (Format)
                {
                    case "daterange":
                        var startDateString = Value.Split('/').FirstOrDefault();
                        if (startDateString != null)
                        {
                            success = DateTime.TryParse(startDateString, out result);

                            if (success)
                            {
                                return DateTime.SpecifyKind(result.ToUniversalTime(), DateTimeKind.Utc);
                            }
                        }
                        return null;
                    case "datetime":
                        success = DateTime.TryParse(Value, out result);

                        if (success)
                        {
                            return DateTime.SpecifyKind(result.ToUniversalTime(), DateTimeKind.Utc);
                        }
                        return null;
                        

                }
                return null;
            }
        }


        public string CollectionName => "Presale";
    }
}
