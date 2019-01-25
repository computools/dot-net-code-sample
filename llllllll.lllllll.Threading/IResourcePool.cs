using System;

namespace *********.**********.Threading
{
	/// <summary>
	///   Provides a pool of resources that can be used to perform a <see cref="IWorkItem">work item</see>.
	/// </summary>
	/// <remarks>
	///   The <see cref="BeginWork"/> method is invoked by an <see cref="IWorkQueue"/> when the
	///   <see cref="WorkItem.State"/> of an <see cref="IWorkItem"/> becomes <see cref="WorkItemState">Scheduled</see>.
	///   <para>
	///   property of the <b>IWorkItem</b> must be set and its <see cref="IWorkItem.State"/> changed
	///   to <see cref="WorkItemState">Failing</see>.
	///   </para>
	/// </remarks>
	/// <example>
	///   The following demonstrates exception handling by a hypothetical thread:
	///   <code>
   ///private WorkLoop
	///{
	///  while (WorkItem workItem = NextWork())
	///  {
   ///   try
   ///   {
   ///    
   ///      // Do the work.
   ///      workItem.State = WorkItemState.Running
   ///      try
   ///      {
   ///        workItem.Perform();
   ///      }
   ///      catch (Exception e)
   ///      {
   ///         // Exception in workitem.
   ///         workItem.FailedException = e;
   ///         workItem.State = WorkItemState.Failing;
   ///      }
   ///   
   ///      // Workitem is done processed, either failed or succeeded.
   ///      workItem.State = WorkItemState.Completed;
   ///   }
   ///   
   ///   catch (Exception e)
   ///   {
   ///     // Internal exception!!!
   ///     workItem.WorkQueue.HandleResourceException(new ResourceExceptionEventArgs(this, e));
   ///   }
   ///  }
   /// }
   ///   </code>
	/// </example>
	public interface IResourcePool
	{
      /// <summary>
      ///   Requests that an <see cref="IWorkItem">work item</see> is performed by a resource
      ///   in the pool.
      /// </summary>
      /// <param name="workItem">
      ///   The <see cref="IWorkItem"/> to execute.
      /// </param>
      /// <remarks>
      ///   <b>BeginWork</b> queues the <paramref name="workItem"/> for execution.  When a resource in the pool
      ///   becomes available, the <see cref="IWorkItem.State"/> of the <paramref name="workItem"/>
      ///   is set to <see cref="WorkItem.State">Running</see>
   
      /// </remarks>
      void BeginWork(IWorkItem workItem);
	}
}
