using System.Collections.Generic;

namespace *********.**********.BusinessServices.Models
{
    public class Facet
    {
        public int Count { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        public List<string> Places { get; set; }
        public List<string> Offers { get; set; }
        public List<string> InventoryTypes { get; set; }
        public List<string> OfferTypes { get; set; }
        public List<string> Accessibility { get; set; }
        public List<string> Shapes { get; set; }
        public List<string> Attributes { get; set; }
    }
}
