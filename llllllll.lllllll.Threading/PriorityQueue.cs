using System;
using System.Collections;

namespace *********.**********.Threading
{
    internal class BinaryPriorityQueue : IPriorityQueue, ICollection, ICloneable, IList
    {
        protected ArrayList InnerList = new ArrayList();
        protected IComparer Comparer;

        public BinaryPriorityQueue() : this(System.Collections.Comparer.Default)
        { }

        public BinaryPriorityQueue(IComparer c)
        {
            Comparer = c;
        }

        public BinaryPriorityQueue(int C) : this(System.Collections.Comparer.Default, C)
        { }

        public BinaryPriorityQueue(IComparer c, int Capacity)
        {
            Comparer = c;
            InnerList.Capacity = Capacity;
        }

        protected BinaryPriorityQueue(ArrayList Core, IComparer Comp, bool Copy)
        {
            if (Copy)
                InnerList = Core.Clone() as ArrayList;
            else
                InnerList = Core;
            Comparer = Comp;
        }



        protected void SwitchElements(int i, int j)
        {
            object h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return Comparer.Compare(InnerList[i], InnerList[j]);
        }

        /// <summary>
        /// Push an object onto the PQ
        /// </summary>
        /// <param name="O">The new object</param>
        /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
        public int Push(object O)
        {
            int p = InnerList.Count, p2;
            InnerList.Add(O); // E[p] = O
            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            return p;
        }

        /// <summary>
        /// Get the smallest object and remove it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public object Pop()
        {
            object result = InnerList[0];
            int p = 0, p1, p2, pn;
            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList.RemoveAt(InnerList.Count - 1);
            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);
            return result;
        }

        /// <summary>
        /// Notify the PQ that the object at position i has changed
        /// and the PQ needs to restore order.
        /// Since you dont have access to any indexes (except by using the
        /// explicit IList.this) you should not call this function without knowing exactly
        /// what you do.
        /// </summary>
        /// <param name="i">The index of the changed object.</param>
        public void Update(int i)
        {
            int p = i, pn;
            int p1, p2;
            do  // aufsteigen
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            if (p < i)
                return;
            do     // absteigen
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);
        }

        /// <summary>
        /// Get the smallest object without removing it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public object Peek()
        {
            if (InnerList.Count > 0)
                return InnerList[0];
            return null;
        }

        public bool Contains(object value)
        {
            return InnerList.Contains(value);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public int Count
        {
            get
            {
                return InnerList.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            InnerList.CopyTo(array, index);
        }

        public object Clone()
        {
            return new BinaryPriorityQueue(InnerList, Comparer, true);
        }

        public bool IsSynchronized
        {
            get
            {
                return InnerList.IsSynchronized;
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return InnerList[index];
            }
            set
            {
                InnerList[index] = value;
                Update(index);
            }
        }

        int IList.Add(object o)
        {
            return Push(o);
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public static BinaryPriorityQueue Syncronized(BinaryPriorityQueue P)
        {
            return new BinaryPriorityQueue(ArrayList.Synchronized(P.InnerList), P.Comparer, false);
        }
        public static BinaryPriorityQueue ReadOnly(BinaryPriorityQueue P)
        {
            return new BinaryPriorityQueue(ArrayList.ReadOnly(P.InnerList), P.Comparer, false);
        }

    }


    internal class HighPriorityQueue : BinaryPriorityQueue
    {
        public HighPriorityQueue() : base(new ReverseComparer())
        { }

        public HighPriorityQueue(int capacity) : base(new ReverseComparer(), capacity)
        { }

        protected HighPriorityQueue(ArrayList Core, IComparer Comp, bool Copy) : base(Core, Comp, Copy)
        {
        }



        public static HighPriorityQueue Syncronized(HighPriorityQueue P)
        {
            return new HighPriorityQueue(ArrayList.Synchronized(P.InnerList), P.Comparer, false);
        }

        public class ReverseComparer : IComparer
        {

            public int Compare(object x, object y)
            {
                return -System.Collections.Comparer.Default.Compare(x, y);
            }


        }
    }

}