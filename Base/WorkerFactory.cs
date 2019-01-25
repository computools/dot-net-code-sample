namespace *********.**********.Core.Base
{
    using *********.**********.BusinessServices.Enums;
    using *********.**********.BusinessServices.Models;
    using *********.**********.Core.Interfaces;
    using *********.**********.Core.ParsingStrategy;
    using *********.**********.Core.Workers;

    public class WorkerFactory
    {
        public IWorker FactoryMethod(TicketType ticketType, Event param, int attempt, string id, bool changeParser)
        {
            switch (ticketType)
            {
                case TicketType.TicketMaster:
                    var worker = new WorkerTicketMasterAsync(param, attempt, id);
                    
                    if (changeParser)
                    {
                        worker.SetParserInstance(new QuickPicksParser());
                    }
                    else
                    {
                        worker.SetParserInstance(new DefaultParser());
                    }

                    return worker;

                case TicketType.LiveNation:
                    var workerliveNation = new WorkerTicketMasterAsync(param, attempt, id);
                    if (changeParser)
                    {
                        workerliveNation.SetParserInstance(new QuickPicksParser());
                    }
                    else
                    {
                        workerliveNation.SetParserInstance(new DefaultParser());
                    }

                    return workerliveNation;

                default: return null;
            }
        }
    }
}
