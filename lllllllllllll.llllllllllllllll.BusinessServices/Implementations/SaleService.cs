using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Models;
using *********.**********.BusinessServices.Repositories;

namespace *********.**********.BusinessServices.Implementations
{
    public class SaleService: ISaleService
    {
        private readonly IMongoRepository _repository;

        public SaleService(IMongoRepository repository)
        {
            _repository = repository;
        }

        public async Task AddPreSales(List<Presale> presales, int bteId)
        {
            foreach (var presale in presales)
            {
                presale.BteId = bteId;
            }
            await _repository.AddRange(presales);
        }
    }
}
