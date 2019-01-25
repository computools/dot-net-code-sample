using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace *********.**********.BusinessServices.WebScrapper
{
    public interface ICefSharpScrapper
    {
        Task<string> WebScraping(string url, string proxyIp, int proxyPort); 
    }
}
