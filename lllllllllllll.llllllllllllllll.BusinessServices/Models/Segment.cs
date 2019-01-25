using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace *********.**********.BusinessServices.Models
{
    public class Image
    {
        public int height { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class LabelStyle
    {
        public string color { get; set; }
        public string font { get; set; }
        public string id { get; set; }
        public int size { get; set; }
    }

    public class Row
    {
        public string id { get; set; }
        public string name { get; set; }
        public int placeSize { get; set; }
        public List<List<object>> placesNoKeys { get; set; }
        public string segmentCategory { get; set; }
        public int totalPlaces { get; set; }
    }

    public class Section
    {
        public string id { get; set; }
        public string name { get; set; }
        public string segmentCategory { get; set; }
        public List<Row> segments { get; set; }
        public int totalPlaces { get; set; }
    }

    public class Label
    {
        public int angle { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public string styleId { get; set; }
        public string text { get; set; }
        public int width { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class Shape
    {
        public List<string> bounds { get; set; }
        public bool filled { get; set; }
        public List<Label> labels { get; set; }
        public string mode { get; set; }
        public string path { get; set; }
    }

    public class Segment
    {
        public bool? generalAdmission { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string segmentCategory { get; set; }
        public List<Section> segments { get; set; }
        public List<Shape> shapes { get; set; }
        public int totalPlaces { get; set; }
    }

    public class Page
    {
        public int height { get; set; }
        public List<Image> images { get; set; }
        public List<LabelStyle> labelStyles { get; set; }
        public List<Segment> segments { get; set; }
        public int width { get; set; }
    }

    public class SegmentCollection
    {
        public List<Page> pages { get; set; }
        public int totalPlaces { get; set; }
        public string venueConfigId { get; set; }
    }

}
