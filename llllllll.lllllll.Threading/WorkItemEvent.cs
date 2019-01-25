using System;

namespace *********.**********.Threading
{
   /// <summary>
   ///   Represents the method that will handle the events associated with an <see cref="IWorkItem"/>.
   /// </summary>
   /// <param name="sender">
   ///   The source of the event.
   /// </param>
   /// <param name="e">
   ///   A <see cref="WorkItemEventArgs"/> than contains the event data.
   /// </param>
   public delegate void WorkItemEventHandler(object sender, WorkItemEventArgs e);

   /// <summary>
   ///   Provides data for the events asscociated with an <see cref="IWorkItem"/>.
   /// </summary>
   public class WorkItemEventArgs : EventArgs
   {
      IWorkItem workItem;

      private WorkItemEventArgs()
      {
      }

      /// <summary>
      ///   Initialise a new instance of the <see cref="WorkItemEventArgs"/> class with the
      ///   specified <see cref="IWorkItem"/>.
      /// </summary>
      /// <param name="workItem">
      ///   The <see cref="IWorkItem"/> associated with the event.
      /// </param>
      /// <remarks>
      ///   Use this constructor to create and initialize a new instance of the <see cref="WorkItemEventArgs"/>
      ///   with the specified <paramref name="workItem"/>.
      /// </remarks>
      public WorkItemEventArgs (IWorkItem workItem) : base()
      {
         this.workItem = workItem;
      }

      /// <summary>
      ///   Gets the <see cref="*********.**********.Threading.WorkItem"/> associated with the event.
      /// </summary>
      /// <value>
      ///   The <see cref="*********.**********.Threading.WorkItem"/> that caused the event.
      /// </value>
      public IWorkItem WorkItem
      {
         get {return workItem;}
      }

   }
}
