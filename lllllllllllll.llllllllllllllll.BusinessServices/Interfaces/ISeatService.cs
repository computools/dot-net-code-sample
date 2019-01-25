using System.Collections.Generic;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface ISeatService
    {
        Task SaveAvailableSeats(List<Facet> facets, List<Seat> seats, List<Offer> offers, string bteId, string url);
        bool MapHasSeats(string html);
        bool MapHasStandingSeats(string html);
    }
}