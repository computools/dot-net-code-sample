using System.Configuration;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Models;
using *********.**********.BusinessServices.Repositories;
using System.Collections.Generic;

namespace *********.**********.BusinessServices.Implementations
{
    public class LogService: MongoBase, ILogService
    {
        private static IMongoRepository _repository;

        public LogService(IMongoRepository repository)
        {
            _repository = repository;
        }

        public async Task AddLog(Log log)
        {
            var addLog = bool.Parse(ConfigurationManager.AppSettings["AddLog"]);
            if (addLog)
            {
                await _repository.Add(log);
            }
        }

        public async Task AddRangeLog(List<Log> log)
        {
            var addLog = bool.Parse(ConfigurationManager.AppSettings["AddLog"]);
            if (addLog)
            {
                await _repository.AddRange(log); 
            }
        }

        public async Task DeleteLogs()
        {
            await Database.DropCollectionAsync("Log");
        }
    }
}
