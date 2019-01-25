namespace *********.**********.Threading
{
   /// <summary>
   ///   Represents the method that will handle the <see cref="WorkQueue.ChangedWorkItemState"/> event.
   /// </summary>
   /// <param name="sender">
   ///   The source of the event.
   /// </param>
   /// <param name="e">
   ///   A <see cref="ChangedWorkItemStateEventArgs"/> than contains the event data.
   /// </param>
   public delegate void ChangedWorkItemStateEventHandler(object sender, ChangedWorkItemStateEventArgs e);

   /// <summary>
   ///   Provides data for the the <see cref="WorkQueue.ChangedWorkItemState"/> event.
   /// </summary>
   public class ChangedWorkItemStateEventArgs : WorkItemEventArgs
   {
      private WorkItemState previousState;

      /// <summary>
      ///   Initialise a new instance of the <see cref="ChangedWorkItemStateEventArgs"/> class with the
      ///   specified <see cref="IWorkItem"/> and <see cref="WorkItemState">previous state</see>.
      /// </summary>
      /// <param name="workItem">
      ///   The <see cref="IWorkItem"/> associated with the event.
      /// </param>
      /// <param name="previousState">
      ///    One of the <see cref="WorkItemState"/> values indicating the previous state of the
      ///    <paramref name="workItem"/>.
      /// </param>
      /// <remarks>
      ///   Use this constructor to create and initialize a new instance of the <see cref="ChangedWorkItemStateEventArgs"/>
      ///   with the specified <paramref name="workItem"/> and <paramref name="previousState"/>.
      /// </remarks>
      public ChangedWorkItemStateEventArgs (IWorkItem workItem, WorkItemState previousState) : base(workItem)
      {
         this.previousState = previousState;
      }

      /// <summary>
      ///   Gets the previous <see cref="WorkItemState"/> associated with the event.
      /// </summary>
      /// <value>
      ///    One of the <see cref="WorkItemState"/> values indicating the previous state of the
      ///    <see cref="WorkItem"/>.
      /// </value>
      public WorkItemState PreviousState
      {
         get {return previousState;}
      }

   }
}
