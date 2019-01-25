using System;
using System.Collections;

namespace *********.**********.Threading
{
    internal interface IPriorityQueue : ICollection, ICloneable, IList
    {
        int Push(object O);
        object Pop();
        object Peek();
        void Update(int i);
    }
}