using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace *********.**********.BusinessServices.Models
{
    public class MetaOffer
    {
        public DateTime modified { get; set; }
        public DateTime expires { get; set; }
    }

    public class SelfOffer
    {
        public string href { get; set; }
    }

    public class EventOffer
    {
        public string href { get; set; }
    }

    public class LinksOffer
    {
        public List<SelfOffer> self { get; set; }
        public List<EventOffer> @event { get; set; }
    }

    public class Meta2Offer
    {
        public string type { get; set; }
        public DateTime modified { get; set; }
        public DateTime expires { get; set; }
    }

    public class ChargeOffer
    {
        public string reason { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
    }

    public class Self2Offer
    {
        public string href { get; set; }
    }

    public class Event2Offer
    {
        public string href { get; set; }
    }

    public class TickettypeOffer
    {
        public string href { get; set; }
    }

    public class Links2Offer
    {
        public List<Self2Offer> self { get; set; }
        public List<Event2Offer> @event { get; set; }
        public List<TickettypeOffer> tickettype { get; set; }
    }

    public class Item
    {
        public string schema { get; set; }
        public Meta2Offer meta { get; set; }
        public string offerId { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public bool online { get; set; }
        public bool @protected { get; set; }
        public bool rollup { get; set; }
        public string inventoryType { get; set; }
        public string offerType { get; set; }
        public string ticketTypeId { get; set; }
        public string auditPriceLevel { get; set; }
        public int? priceLevelId { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string listPrice { get; set; }
        public string faceValue { get; set; }
        public decimal? totalPrice { get; set; }
        public List<ChargeOffer> charges { get; set; }
        public List<int> sellableQuantities { get; set; }
        public List<object> alternateIds { get; set; }
        public Links2Offer _links { get; set; }
    }

    public class EmbeddedOffer
    {
        public List<Item> item { get; set; }
    }

    public class OfferModal
    {
        public string schema { get; set; }
        public MetaOffer meta { get; set; }
        public LinksOffer _links { get; set; }
        public EmbeddedOffer _embedded { get; set; }
    }
}
