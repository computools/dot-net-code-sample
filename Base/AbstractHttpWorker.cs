// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractHttpWorker.cs" company="">
//
// </copyright>
// <summary>
//   AbstractHttpWorker.cs
//   Was added during first step of refactoring. We don't use at now
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace *********.**********.Core.Base
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using *********.**********.BusinessServices.Models;
    using *********.**********.Core.Configurations;
    using *********.**********.Core.Model;

    public abstract class AbstractHttpWorker : AbstractWorker
    {
        protected AbstractHttpWorker(Event param, int attempt, string id) : base(param, attempt, id)
        {
            this.Param = param;
            this.Attempt = attempt;
            this.Id = id;
        }
    }
}
