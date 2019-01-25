using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace *********.**********.Threading
{
    
    public class WorkQueue : IWorkQueue
    {
        private HighPriorityQueue queue;
        private IResourcePool resourcePool;
        private object completed = new object();
        private bool pausing;

        private int runningItems;
        private volatile Exception internalException;

        
        private readonly object eventLock = new object();


    
        public WorkQueue()
        {
            queue = new HighPriorityQueue();
            StopWatch = new Stopwatch();

        }

        public Stopwatch StopWatch { get; set; }

        public DateTime FirstItemStartedOn { get; set; }

        
        public IResourcePool WorkerPool
        {
            get
            {
                if (resourcePool == null)
                {
                    resourcePool = WorkThreadPool.Default;
                }
                return resourcePool;
            }
            set
            {
                resourcePool = value;
            }
        }
        
        public int Count
        {
            get { return runningItems + queue.Count; }
        }

        public void Add(IWorkItem workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException("workItem");
            }

            if (internalException != null)
            {
                throw new NotSupportedException("WorkQueue encountered an internal error.", internalException);
            }

            if (queue.Count == 0)
            {
                StopWatch.Start();
                FirstItemStartedOn = DateTime.UtcNow;
            }

            // Assign it to this queue.
            workItem.WorkQueue = this;

            // Can we schedule it for execution now?
            lock (this)
            {
                if (!pausing && runningItems < ConcurrentLimit)
                {
                    workItem.State = WorkItemState.Scheduled;
                }
                else
                {
                    // Add the workitem to queue.
                    queue.Push(workItem);
                    workItem.State = WorkItemState.Queued;
                }
            }
        }

        private bool DoNextWorkItem()
        {
            lock (this)
            {
                // Get some work and start it.
                if (!pausing && runningItems < ConcurrentLimit && queue.Count != 0)
                {
                    IWorkItem item = (IWorkItem)queue.Pop();
                    item.State = WorkItemState.Scheduled;
                    return true;
                }
            }

            return false;
        }
        
        public int ConcurrentLimit { get; set; } = 200;
        

       
        public void Pause()
        {
            Pausing = true;
        }
        
        public void Resume()
        {
            Pausing = false;
        }
        
        public void Clear()
        {
            lock (this)
            {
                queue.Clear();
            }
        }
        
        public bool Pausing
        {
            get { return pausing; }
            set
            {
                if (pausing != value)
                {
                    pausing = value;

                    // Start executing some work.
                    while (!pausing && DoNextWorkItem())
                    {
                    }
                }
            }
        }
        
        public void WaitAll()
        {
            lock (this)
            {
                if (internalException != null)
                    throw internalException;

                if (pausing)
                    throw new InvalidOperationException("The queue is paused, no work will be performed.");

                if (runningItems == 0 && queue.Count == 0)
                    return;
            }

            lock (completed)
            {
                if (internalException != null)
                    throw internalException;

                if (runningItems == 0 && queue.Count == 0)
                    return;

                Monitor.Wait(completed);

                if (internalException != null)
                    throw internalException;
            }
        }
        
        public bool WaitAll(TimeSpan timeout)
        {
            lock (this)
            {
                if (internalException != null)
                    throw internalException;
            }

            lock (completed)
            {
                if (!Monitor.Wait(completed, timeout))
                    return false;

                if (internalException != null)
                    throw internalException;
            }

            return true;
        }

        
        public event ChangedWorkItemStateEventHandler ChangedWorkItemState
        {
            add
            {
                lock (eventLock)
                {
                    changedWorkItemState += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    changedWorkItemState -= value;
                }
            }
        }
        private ChangedWorkItemStateEventHandler changedWorkItemState;
       
        protected virtual void OnChangedWorkItemState(IWorkItem workItem, WorkItemState previousState)
        {
            ChangedWorkItemStateEventHandler handler;

            lock (eventLock)
            {
                handler = changedWorkItemState;
            }
            if (handler != null)
            {
                handler(this, new ChangedWorkItemStateEventArgs(workItem, previousState));
            }
        }

        public event EventHandler AllWorkCompleted
        {
            add
            {
                lock (eventLock)
                {
                    allWorkCompleted += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    allWorkCompleted -= value;
                }
            }
        }
        private EventHandler allWorkCompleted;

       
        protected virtual void OnAllWorkCompleted(EventArgs e)
        {
            EventHandler handler;

            lock (eventLock)
            {
                handler = allWorkCompleted;
            }
            if (handler != null)
            {
                handler(this, e);
            }
        }

        
        public event WorkItemEventHandler RunningWorkItem
        {
            add
            {
                lock (eventLock)
                {
                    runningWorkItem += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    runningWorkItem -= value;
                }
            }
        }
        private event WorkItemEventHandler runningWorkItem;

        
        protected virtual void OnRunningWorkItem(IWorkItem workItem)
        {
            WorkItemEventHandler handler;

            lock (eventLock)
            {
                handler = runningWorkItem;
            }
            if (handler != null)
            {
                handler(this, new WorkItemEventArgs(workItem));
            }
        }


        /// <summary>
        ///   Occurs when an <see cref="IWorkItem"/> has completed execution.
        /// </summary>
        public event WorkItemEventHandler CompletedWorkItem
        {
            add
            {
                lock (eventLock)
                {
                    completedWorkItem += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    completedWorkItem -= value;
                }
            }
        }
        private event WorkItemEventHandler completedWorkItem;
        
        protected virtual void OnCompletedWorkItem(IWorkItem workItem)
        {
            WorkItemEventHandler handler;

            lock (eventLock)
            {
                handler = completedWorkItem;
            }
            if (handler != null)
            {
                handler(this, new WorkItemEventArgs(workItem));
            }
        }


        public event WorkItemEventHandler FailedWorkItem
        {
            add
            {
                lock (eventLock)
                {
                    failedWorkItem += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    failedWorkItem -= value;
                }
            }
        }
        private event WorkItemEventHandler failedWorkItem;
        
        protected virtual void OnFailedWorkItem(IWorkItem workItem)
        {
            WorkItemEventHandler handler;

            lock (eventLock)
            {
                handler = failedWorkItem;
            }
            if (handler != null)
            {
                handler(this, new WorkItemEventArgs(workItem));
            }
        }

        
        public event ResourceExceptionEventHandler WorkerException
        {
            add
            {
                lock (eventLock)
                {
                    workerException += value;
                }
            }
            remove
            {
                lock (eventLock)
                {
                    workerException -= value;
                }
            }
        }
        private event ResourceExceptionEventHandler workerException;
        
        protected virtual void OnWorkerException(ResourceExceptionEventArgs e)
        {
            ResourceExceptionEventHandler handler;

            lock (eventLock)
            {
                handler = workerException;
            }
            if (handler != null)
                handler(this, e);
        }

        
        public void WorkItemStateChanged(IWorkItem workItem, WorkItemState previousState)
        {
            OnChangedWorkItemState(workItem, previousState);

            switch (workItem.State)
            {
                case WorkItemState.Scheduled:
                    lock (this)
                    {
                        // Housekeeping chores.
                        ++runningItems;

                        // Now start it.
                        
                        WorkerPool.BeginWork(workItem);
                    }
                    break;

                case WorkItemState.Running:
                    OnRunningWorkItem(workItem);
                    break;

                case WorkItemState.Failing:
                    OnFailedWorkItem(workItem);
                    break;

                case WorkItemState.Completed:
                    bool allDone = false;
                    lock (this)
                    {
                        --runningItems;
                        allDone = queue.Count == 0 && runningItems == 0;
                    }

                    // Tell the world that the workitem has completed.
                    if (queue.Count < ConcurrentLimit / 2 && (ConcurrentLimit-  runningItems)*100/ConcurrentLimit > 5 )
                    {
                        OnCompletedWorkItem(workItem);
                    }
                    

                    // Find some more work.
                    if (allDone)
                    {
                        // Wakeup.
                        //OnAllWorkCompleted(EventArgs.Empty);
                        lock (completed)
                        {
                            Monitor.PulseAll(completed);
                        }
                    }
                    else
                    {
                        DoNextWorkItem();
                    }
                    break;
            }

        }

        
        public void HandleResourceException(ResourceExceptionEventArgs e)
        {
            lock (completed)
            {
                Pause();
                internalException = e.Exception;

                // Tell the world.
                OnWorkerException(e);

                // Wakeup any threads in WaitAll and let them throw the exception.
                Monitor.PulseAll(completed);
            }

        }


    }
}
