using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace *********.**********.BusinessServices.Implementations
{
    public static class StreamService
    {
        public static Dictionary<string, Stream> Streams;
        public static List<string> Files { get; set; }
        static StreamService()
        {
            Files = new List<string>
            {
                /*
                HIDDEN
                */
            };

            Streams = new Dictionary<string, Stream>();

            foreach (var file in Files)
            {
                Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                Streams.Add(file, stream);
            }
        }

        public static Stream GetStream(string name)
        {
            Stream resultStream = new MemoryStream();
            Streams[name].CopyTo(resultStream);
            return resultStream;
        }
    }
}
