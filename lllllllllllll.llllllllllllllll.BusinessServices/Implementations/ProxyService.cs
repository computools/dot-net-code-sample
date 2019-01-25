using System;
using System.Collections.Generic;
using *********.**********.BusinessServices.Models;
using MongoDB.Driver;
using System.Linq;
using *********.**********.BusinessServices.Enums;
using System.Configuration;

namespace *********.**********.BusinessServices.Implementations
{
    public static class ProxyService
    {
        private static readonly Random Random = null;
        private static List<Proxy> Proxies { get; set; }
        static ProxyService()
        {
            Random = new Random();
        }

        public static void Load()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["GathererPrimary"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["GathererDatabaseName"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<Proxy>("Proxy");
            Proxies = collection.Find(_ => true).ToList();
        }

        public static Models.Proxy GetRandom()
        {
            var successProxies = Proxies.Where(x => x.Status == SiteResponse.Success || x.Status == SiteResponse.StaticMap || x.Status == SiteResponse.DeletedEvent || x.Status == SiteResponse.NoOffers || x.Status == SiteResponse.EventOffsale).ToList();
            if (successProxies.Any())
            {
                var proxy = successProxies.Where(x=> DateTime.UtcNow - x.LastUsed > new TimeSpan(0,0,20)).OrderBy(x => x.LastUsed).FirstOrDefault();
                if (proxy != null)
                {
                    proxy.Status = SiteResponse.None;
                    proxy.Reused = true;
                    return proxy;
                }
            }
            var proxyResult = Proxies.Count == 0 ? null : Proxies[Random.Next(0, Proxies.Count)];
            proxyResult.Status = SiteResponse.None;
            return proxyResult;
        }
    }
}
