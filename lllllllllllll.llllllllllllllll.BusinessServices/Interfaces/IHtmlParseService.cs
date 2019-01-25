using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface IHtmlParseService
    {
        Tuple<string,string> ParseApiData(string html);
        List<Offer> ParseOffers(string html);
        List<Facet> ParseFacets(Task<string> htmlTask);
        
        List<Seat> ParseSeats(string html);
        List<Presale> ParsePresales(string html);
        
        List<Seat> ParseStandingSeats(string html);

        Task<List<Seat>> ParseSeatsInfoJson(string htmlTask);
        Task<List<Offer>> ParseOffersInfoJson(string htmlTask);
    }
}
