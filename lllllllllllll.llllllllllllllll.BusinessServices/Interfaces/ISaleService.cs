using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Models;

namespace *********.**********.BusinessServices.Interfaces
{
    public interface ISaleService
    {
        Task AddPreSales(List<Presale> presales,int bteId);
    }
}
