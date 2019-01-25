using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface ILogService
    {
        Task AddLog(Log log);

        Task AddRangeLog(List<Log> log);

        Task DeleteLogs();
    }
}
