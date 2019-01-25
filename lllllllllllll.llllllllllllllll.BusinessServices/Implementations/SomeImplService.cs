using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Repositories;

namespace *********.**********.BusinessServices.Implementations
{
    public class SomeImplService : **** , ITicketService
    {

        public SomeImplService(IMongoRepository repository, ILogService logService) : base(repository, logService)
        {
            
        }

        public SomeImplService(IHtmlParseService htmlParseService, IMongoRepository repository, IEventService eventService, ILogService logService, ISeatService seatService, ISaleService saleService) : base(htmlParseService, repository, eventService, logService, seatService, saleService)
        {
            _eventUrl =    /*HIDDEN*/
        }
    }
}
