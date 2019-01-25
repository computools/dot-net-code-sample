using System;
using System.Diagnostics;

namespace *********.**********.Threading
{
	/// <summary>
	///   Provides the methods and properties to manage the scheduling of an <see cref="IWorkItem">work item</see>.
	/// </summary>
	/// <remarks>
	///   <b>IWorkQueue</b> provides the methods and properties to the manage the scheduling of an <see cref="IWorkItem"/>.
	///   Its primary responsibility is to determine when and it what order work items are executed.
	///   <para>
	///   The <see cref="WorkItemStateChanged"/> method is invoked by an <see cref="IWorkItem"/> to inform the <b>WorkQueue</b>
	///   of a <see cref="IWorkItem.State"/> change.  It is the responsible of the <b>WorkQueue</b> to
	///   perform the appropiate logic for the given state.
	///   </para>
	///   <para>
	///   </para>
	/// </remarks>
   public interface IWorkQueue
   {
      /// <summary>
      ///   Invoked by an <see cref="IWorkItem"/> to inform a work queue that its <see cref="IWorkItem.State"/>
      ///   has changed.
      /// </summary>
      /// <param name="workItem">
      ///   The <see cref="IWorkItem"/> that has changed <see cref="IWorkItem.State"/>.
      /// </param>
      /// <param name="previousState">
      ///    One of the <see cref="WorkItemState"/> values indicating the previous state of the <paramref name="workItem"/>.
      /// </param>
      /// <remarks>
      ///   It is the responsible of the <see cref="IWorkQueue"/> to  perform the appropiate logic for the 
      ///   new <see cref="IWorkItem.State"/>.
      /// </remarks>
      void WorkItemStateChanged (IWorkItem workItem, WorkItemState previousState);

      /// <summary>
      ///   Invoked by an <see cref="IResourcePool"/> when an exception is thrown outside of normal
      ///   processing.
      /// </summary>
      /// <param name="e">
      ///   A <see cref="ResourceExceptionEventArgs"/> that contains the event data.
      /// </param>
      /// <seealso cref="IResourcePool"/>
      void HandleResourceException(ResourceExceptionEventArgs e);

	    /// <summary>
	    ///   Add some work to execute.
	    /// </summary>
	    /// <param name="workItem">
	    ///   An <see cref="IWorkItem"/> to execute.
	    /// </param>
	    /// <remarks>
	    ///   If the ConcurrentLimit is not reached and not Pause pausing
	    ///   then the <paramref name="workItem"/>
	    ///   is immediately executed on the WorkerPool.  Otherwise it is placed in a
	    ///   holding queue and executed when another <see cref="IWorkItem"/> completes.
	    /// </remarks>
	    void Add(IWorkItem workItem);


        Stopwatch StopWatch { get; set; }

        DateTime FirstItemStartedOn { get; set; }
   }
}
