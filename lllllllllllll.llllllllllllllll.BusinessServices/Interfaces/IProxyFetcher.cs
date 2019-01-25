using *********.**********.BusinessServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface IProxyFetcher
    {
        void GetProxiesFromDatabase();
        WebProxyWrap GetProxy();

        List<WebProxyWrap> GetAllProxies();
    }
}
