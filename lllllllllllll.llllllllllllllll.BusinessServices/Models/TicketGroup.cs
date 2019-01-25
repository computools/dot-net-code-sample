using System.Collections.Generic;

namespace *********.**********.BusinessServices.Models
{
    public class TicketGroup
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string StartSeatNumber { get; set; }
        public string EndSeatNumber { get; set; }
        public int SeatQty { get; set; }
        public List<string> SeatTypes { get; set; }
        public bool? ObstructedView { get; set; }
        public List<string> Accessibility { get; set; }
        public List<string> InventoryTypes { get; set; }
        public List<Offer> Offers { get; set; }
    }
}