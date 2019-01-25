using System;

namespace *********.**********.Threading
{
   /// <summary>
   ///   Represents the method that will handle the <see cref="WorkQueue.WorkerException"/> 
   ///   and <see cref="WorkThreadPool.ThreadException"/> events.
   /// </summary>
   /// <param name="sender">
   ///   The source of the event.
   /// </param>
   /// <param name="e">
   ///   A <see cref="ResourceExceptionEventArgs"/> than contains the event data.
   /// </param>
   public delegate void ResourceExceptionEventHandler(object sender, ResourceExceptionEventArgs e);

   /// <summary>
   ///   Provides data for the <see cref="WorkQueue.WorkerException"/> 
   ///   and <see cref="WorkThreadPool.ThreadException"/> events.
   /// </summary>
 
   public sealed class ResourceExceptionEventArgs : EventArgs
   {
      private object resource;
      private System.Exception exception;
      private IWorkItem workItem;

      private ResourceExceptionEventArgs()
      {
      }

      /// <summary>
      ///   Initialise a new instance of the <see cref="ResourceExceptionEventArgs"/> class with the
      ///   specified resource and <see cref="System.Exception"/>.
      /// </summary>
      /// <param name="resource">
      ///   The <see cref="object"/> that raised the exception.
      /// </param>
      /// <param name="exception">
      ///   The <see cref="System.Exception"/> that occured.
      /// </param>
      /// <remarks>
      ///   Use this constructor to create and initialize a new instance of the <see cref="ResourceExceptionEventArgs"/>
      ///   with the specified <see cref="System.Exception"/>.
      /// </remarks>
      public ResourceExceptionEventArgs (object resource, Exception exception) : base()
      {
         this.resource = resource;
         this.exception = exception;
      }

      /// <summary>
      ///   Initialise a new instance of the <see cref="ResourceExceptionEventArgs"/> class with the
      ///   specified resource, <see cref="IWorkItem"/> and <see cref="System.Exception"/>.
      /// </summary>
      /// <param name="resource">
      ///   The <see cref="object"/> that raised the exception.
      /// </param>
      /// <param name="workItem">
      ///   The <see cref="IWorkItem"/> that the <paramref name="resource"/> was working on.
      /// </param>
      /// <param name="exception">
      ///   The <see cref="System.Exception"/> that occured.
      /// </param>
      /// <remarks>
      ///   Use this constructor to create and initialize a new instance of the <see cref="ResourceExceptionEventArgs"/>
      ///   with the specified <see cref="System.Exception"/>.
      /// </remarks>
      public ResourceExceptionEventArgs (object resource, IWorkItem workItem, Exception exception) : base()
      {
         this.resource = resource;
         this.workItem = workItem;
         this.exception = exception;
      }

      /// <summary>
      ///   Gets the exception that occured.
      /// </summary>
      /// <value>
      ///   The <see cref="System.Exception"/> that occured.
      /// </value>
      public System.Exception Exception
      {
         get {return exception;}
      }

      /// <summary>
      ///   Gets the work item.
      /// </summary>
      /// <value>
      ///   A <see cref="IWorkItem"/> or <b>null</b>.
      /// </value>
      public IWorkItem WorkItem
      {
         get {return workItem;}
      }

      /// <summary>
      ///   Gets the resource that raised the exception.
      /// </summary>
      /// <remarks>
      ///   A <b>Resource</b> is something that can perform <see cref="IWork">work</see>.
      /// </remarks>
      public object Resource
      {
         get {return resource;}
      }

   }
}
