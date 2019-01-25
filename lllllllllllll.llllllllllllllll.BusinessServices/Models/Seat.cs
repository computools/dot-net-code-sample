using System.Collections.Generic;
using System.Linq;

namespace *********.**********.BusinessServices.Models
{
    public class Seat
    {
        public Seat()
        {
            Offers = new List<Offer>();
            OfferTypes = new List<string>();
            Accessibility = new List<string>();
            InventoryTypes = new List<string>();
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Row { get; set; }
        public string Section { get; set; }
        public List<string> SeatTypes => OfferTypes.Distinct().ToList();
        public List<Offer> Offers { get; set; }
        public List<string> OfferTypes { get; set; }
        public string Description { get; set; }
        public Offer Offer
        {
            get { return Offers.FirstOrDefault(x => x.TotalPrice > 0); }
        }
        public bool IsAvailable { get; set; }
        public List<string> Accessibility { get; set; }
        public List<string> InventoryTypes { get; set; }
        public bool IsStanding { get; set; }

    }
}
