// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractWorker.cs" company="">
//
// </copyright>
// <summary>
//   AbstractWorker.cs is a head class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace *********.**********.Core.Base
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using BestTime.Data.Common;
    using *********.**********.BusinessServices.Interfaces;
    using *********.**********.BusinessServices.Models;
    using *********.**********.Core.Configurations;
    using *********.**********.Core.Interfaces;
    using *********.**********.Core.LoggerEntity;
    using *********.**********.Core.Model;
    using *********.**********.Core.Storages;
    using *********.**********.Core.Storages.Enums;

    public abstract class AbstractWorker : IWorker
    {
        private readonly IPeriodicDbUploader<TicketGroupModel> _mongoSuccessUploader;
        private readonly IEventService _eventService;
        
        protected AbstractWorker(Event param)
        {
            this.Param = param;
            this._eventService = ServiceLocator.GetService<IEventService>();
            this._mongoSuccessUploader = ServiceLocator.GetService<IPeriodicDbUploader<TicketGroupModel>>();
            this.Id = Guid.NewGuid().ToString();
        }

        protected AbstractWorker(Event param, int attempt, string id)
        {
            this.Param = param;
            this._eventService = ServiceLocator.GetService<IEventService>();
            this._mongoSuccessUploader = ServiceLocator.GetService<IPeriodicDbUploader<TicketGroupModel>>();
            this.Id = id;
            this.Attempt = attempt;
        }

        protected Event Param { get; set; }

        protected int Attempt { private get; set; }

        protected string Id { private get; set; }

        protected IParserExecutor ParserInstance { get; set; }

        /// <summary>
        /// Start performing of new worker.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Perform()
        {
            Interlocked.Increment(ref Balancer.Balancer.SimultaneousWorkers); // on start increase value
            try
            {
                var processedData = await this.DoParseWorkAsync().ConfigureAwait(false);

                await this.PutWorkerIntoStorage(processedData.Status).ConfigureAwait(false);
                
                await ServiceLocator.GetService<IPeriodicDbUploader<Log>>().SetLogData(
                    new Log()
                        {
                            IsPulse = false,
                            CreatedOn = DateTime.UtcNow,
                            EventId = this.Param.EventId,
                            EventUrl = this.Param.Url,
                            Message = ProcessStatusToString.GetLogStatus(processedData.Status,string.Empty)
                        }).ConfigureAwait(false);


                if (processedData.Success)
                {
                    await this._mongoSuccessUploader.SetLogData(processedData.Value)
                        .ConfigureAwait(false); // save success result into service

                    await this._eventService.SetNextTimeEvent(this.Param.EventId, true)
                        .ConfigureAwait(false); // set NextTime value for event
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Info(
                    $"Exception : {ex.Message}, link: {this.Param.Url}, workerid {this.Id}, eventId: {this.Param.EventId}, stack trace: {ex.StackTrace} inner exception: {ex.InnerException?.Message}");
            }
            finally
            {
                Interlocked.Decrement(ref Balancer.Balancer.SimultaneousWorkers); // if worker was finished decrease value 
                await Task.Run(() => Balancer.Balancer.LaunchNewWorker()).ConfigureAwait(false);
            }
        }

        public void SetParserInstance(IParserExecutor parserInstance)
        {
            this.ParserInstance = parserInstance;
        }

        protected abstract Task<ResultTicketMaster> DoParseWorkAsync();
       

        private async Task PutWorkerIntoStorage(ProcessingStatus status)
        {
            Logger.Log.Info($"EventId : {this.Param.EventId}, status: {status}, workerid {this.Id}");

            // case EmptyResponseNoSeats means that we have to change ParserInstance to parse this event correctly
            // refer to non interactive maps 
            if (status == ProcessingStatus.EmptyResponseNoSeats) 
            {
                Logger.Log.Info(
                    $"EventId : {this.Param.EventId}, parser changed, link: {this.Param.Url}, workerid {this.Id}");

                await Task.Run(
                    () => ServiceLocator.GetService<IStorage<EventWorkerModel>>().Add(
                        new EventWorkerModel(this.Param, this.Attempt++, this.Id, true))).ConfigureAwait(false);
            }
          
        }
    }
}
