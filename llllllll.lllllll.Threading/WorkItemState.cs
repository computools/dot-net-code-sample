using System;
using System.Threading;

namespace *********.**********.Threading
{
    /// <summary>
    ///   Specifies the state of a <see cref="WorkItem"/>.
    /// </summary>
    /// <remarks>
    ///   <img src="WorkItemState.png" alt="WorkItem state transistions"/>
    /// </remarks>
    public enum WorkItemState
    {
        /// <summary>
        ///   Not assigned to a <see cref="WorkQueue"/>.
        /// </summary>
        Created = 0,

        /// <summary>
        ///   Waiting for a <see cref="Thread"/> to execute on.
        /// </summary>
        Scheduled,

        /// <summary>
        ///   Waiting for another <see cref="WorkItem"/> to complete, so it can run concurrently.
        /// </summary>
        Queued,

        /// <summary>
        ///   Executing on a <see cref="Thread"/>.
        /// </summary>
        Running,

        /// <summary>
        ///   Recovering from a thrown <see cref="Exception"/>.
        /// </summary>
        Failing,

        /// <summary>
        ///   Finished executing.
        /// </summary>
        Completed
    }
}
