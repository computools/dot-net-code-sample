using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace *********.**********.Threading
{

    /// <summary>
    ///   Represents some work that must be performed.
    /// </summary>
    /// <remarks>
    ///   The order of execution of <b>WorkItems</b> is determined by the <see cref="Priority"/> property.
    ///   Higher valued priorities will execute earlier.
    ///   <para>
    ///   </para>
    /// </remarks>
    public abstract class WorkItem : IWorkItem, IComparable
    {
        
        private Exception _failedException;
        private WorkItemState _state;
        private IWorkQueue _workQueue;
        
        private CultureInfo uiCulture;

        /// <summary>
        ///   Creates a new instance of the <see cref="WorkItem"/> class.
        /// </summary>
        protected WorkItem()
        {
            Priority = ThreadPriority.Normal;
            _state = WorkItemState.Created;

            // Capture the invokers context.
            uiCulture = Thread.CurrentThread.CurrentUICulture;
        }




        /// <summary>
        ///   Gets or sets the <see cref="IWorkQueue"/> containing this <see cref="IWorkItem"/>.
        /// </summary>
        /// <value>
        ///   The <see cref="IWorkQueue"/> that is scheduling this <see cref="IWorkItem"/>.
        /// </value>
        public IWorkQueue WorkQueue
        {
            get { return _workQueue; }
            set
            {
                if (_workQueue != value)
                {
                    if (_workQueue != null)
                        throw new NotSupportedException(String.Format("'{0}' is assigned to another WorkQueue '{1}'.",
                            this, _workQueue));

                    _workQueue = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref="WorkItemState">state</see>.
        /// </summary>
        /// <value>
        ///   One of the <see cref="WorkItemState"/> values indicating the state of the current <b>WorkItem</b>. 
        ///   The initial value is <b>Created</b>.
        /// </value>
        /// <exception cref="InvalidTransitionException">
        ///   The <b>State</b> can not be transitioned to <paramref name="value"/>.
        /// </exception>
        /// <remarks>
        ///   The <b>State</b> represents where the <see cref="WorkItem"/> is in processing pipeline.
        ///   The following transition can take place:
        ///   <para>
        ///   <img src="WorkItemState.png" alt="WorkItem state transistions"/>
        ///   </para>
        ///   <para>
        ///   </para>
        /// </remarks>
        /// <seealso cref="ValidateStateTransition"/>
        public WorkItemState State
        {
            get { return _state; }
            set
            {
                ValidateStateTransition(_state, value);

                WorkItemState prev = _state;
                _state = value;
                switch (_state)
                {
                    case WorkItemState.Running:
                        
                        ApplyInvokerContext();
                        break;

                    case WorkItemState.Completed:
                        
                        break;
                }

                WorkQueue?.WorkItemStateChanged(this, prev);
            }
        }

        /// <summary>
        ///   Validate a state transition.
        /// </summary>
        /// <param name="currentState">
        ///   One of the <see cref="WorkItemState"/> values indicating the current <see cref="WorkItemState"/>. 
        /// </param>
        /// <param name="nextState">
        ///   One of the <see cref="WorkItemState"/> values indicating the requested <see cref="WorkItemState"/>. 
        /// </param>
        /// <exception cref="InvalidTransitionException">
        ///   The transition from current to  value is invalid.
        /// </exception>
        /// <remarks>
        ///   <b>ValidateStateTransition</b> throws <see cref="InvalidTransitionException"/> if
        ///   the transition from current to  value is invalid.
        ///   <para>
        ///   Derived class can use this method for extra validation.
        ///   </para>
        /// </remarks>
        protected virtual void ValidateStateTransition(WorkItemState currentState, WorkItemState nextState)
        {
            //I don't think this method is useful but keeping it in here, the retries screw everything up

            switch (currentState)
            {
                case WorkItemState.Completed:
                    //handles rescheduling cases
                    if (nextState == WorkItemState.Scheduled || nextState == WorkItemState.Queued || nextState == WorkItemState.Created || nextState == WorkItemState.Completed )
                        return;
                    break;

                case WorkItemState.Created:
                    if (nextState == WorkItemState.Scheduled || nextState == WorkItemState.Queued)
                        return;
                    break;


                case WorkItemState.Failing:
                    if (nextState == WorkItemState.Completed || nextState == WorkItemState.Scheduled)
                        return;
                    break;

                case WorkItemState.Queued:
                    if (nextState == WorkItemState.Scheduled)
                        return;
                    break;

                case WorkItemState.Running:
                    if (nextState == WorkItemState.Completed || nextState == WorkItemState.Failing)
                        return;
                    break;

                case WorkItemState.Scheduled:
                    if (nextState == WorkItemState.Running)
                        return;
                    break;

                default:
                    break;
            }

            //throw new InvalidTransitionException(this, currentState, nextState);
        }

        /// <summary>
        ///   Gets or sets the <see cref="Exception"/> that caused the <see cref="WorkItem"/> to
        ///   failed.
        /// </summary>
        public Exception FailedException
        {
            get { return _failedException; }
            set { _failedException = value; }
        }

        /// <summary>
        ///   Gets or sets the scheduling priority.
        /// </summary>
        /// <value>
        ///   One of the <see cref="ThreadPriority"/> values. The default value is <b>Normal</b>.
        /// </value>
        /// <remarks>
        ///   <b>Prioriry</b> specifies the relative importance of one <see cref="WorkItem"/> versus another.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/> is not valid.
        /// </exception>
        public ThreadPriority Priority { get; set; }
        


        /// <summary>
        ///   Perform the work.
        /// </summary>
        /// <remarks>
        ///   <b>Perform</b> performs the work. 
        ///   <para>
        ///   Before the method is called, the <see cref="CultureInfo.CurrentUICulture"/> of the <i>invoker</i>
        ///   is applied to this <see cref="Thread"/>.  The <i>invoker's</i> culture is capture when 
        ///   the <see cref="WorkItem"/> is constructed.
        ///   </para>
        ///   <para>
        ///   A thrown <see cref="Exception"/> is caught by the <see cref="IResourcePool"/> and the
        ///   workitem's
        ///   <see cref="FailedException"/> property is set and its <see cref="State"/> changed
        ///   to <see cref="WorkItemState">Failing</see>.
        ///   </para>
        ///   <para>
        ///   This is an <b>abstract</b> method and must be implmented by derived classes.
        ///   </para>
        /// </remarks>
        public abstract Task Perform();

        /// <summary>
        ///   Changes the "context" to the context of creator of the <see cref="WorkItem"/>.
        /// </summary>
        /// <remarks>
        ///   The <see cref="CultureInfo.CurrentUICulture"/> of the <i>invoker</i> is applied
        ///   to this <see cref="Thread"/>.  The <i>invoker</i> context is defined when 
        ///   the <see cref="WorkItem"/> is constructed.
        /// </remarks>
        internal void ApplyInvokerContext()
        {
            Thread thisThread = Thread.CurrentThread;

            if (uiCulture != thisThread.CurrentUICulture)
                thisThread.CurrentUICulture = uiCulture;
        }


      
        public int CompareTo(object obj)
        {
            WorkItem wi = obj as WorkItem;
            if (wi == null)
                throw new ArgumentException("Not a WorkItem.");

            return this.Priority - wi.Priority;
        }



        
    }
}
