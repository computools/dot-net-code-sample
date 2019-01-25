using System;
using System.Threading;

namespace *********.**********.Threading
{

   /// <summary>
   ///   Defines an interface to allow work to be managed by an <see cref="IWorkQueue"/>
   /// </summary>
   /// <remarks>
   ///   <b>IWorkItem</b> extends the <see cref="IWork"/> interface, allowing work to be managed by 
   ///   a <see cref="IWorkQueue">work queue</see>.
   /// </remarks>
   public interface IWorkItem : IWork
   {
      /// <summary>
      ///   Gets or sets the <see cref="IWorkQueue"/> that manages this <see cref="IWorkItem"/>.
      /// </summary>
      /// <value>
      ///   The <see cref="IWorkQueue"/> that is scheduling this <see cref="IWorkItem"/>.
      /// </value>
      IWorkQueue WorkQueue {get; set;}
      
           /// <summary>
      ///   Gets or sets the <see cref="WorkItemState">state</see>.
      /// </summary>
      /// <value>
      ///   One of the <see cref="WorkItemState"/> values indicating the state of the current <b>WorkItem</b>. 
      ///   The initial value is <b>Created</b>.
      /// </value>
      /// <remarks>
      ///   The <b>State</b> represents where the <see cref="WorkItem"/> is in processing pipeline.
      ///   The following transition can take place:
      ///   <para>
      ///   <img src="WorkItemState.png" alt="WorkItem state transistions"/>
      ///   </para>
      ///   <para>

      ///   </para>
      /// </remarks>
      WorkItemState State {get; set;}

      /// <summary>
      ///   Gets or sets the <see cref="Exception"/> that caused the <see cref="WorkItem"/> to
      ///   failed.
      /// </summary>
      Exception FailedException {get; set;}

      /// <summary>
      ///   Gets or sets the scheduling priority.
      /// </summary>
      /// <value>
      ///   One of the <see cref="ThreadPriority"/> values. The default value is <b>Normal</b>.
      /// </value>
      /// <remarks>
      ///   <b>Prioriry</b> specifies the relative importance of one <see cref="WorkItem"/> versus another.
      /// </remarks>
      ThreadPriority Priority {get; set;}
   }
}
