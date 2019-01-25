using System.Threading.Tasks;
using *********.**********.BusinessServices.Enums;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface ITicketService
    {
        Task<SiteResponse> GetEventTickets(Event @event, Proxy proxy, int attempt);
    }
}
