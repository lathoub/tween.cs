using System;
using System.Collections;

namespace Tween
{
    public class TweenEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        public TweenEventArgs(Hashtable obj)
        {
            Obj = obj;
        }

        public Hashtable Obj { get; private set; }
    }
}