using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This is an enumerator of pigeon actions. 
    /// It is good programming practice to use enumerators and DO NOT use fixed strings in code ;)
    /// </summary>
    public enum PigeonAction
    {
        Waiting,
        WalkingLeft,
        WalkingRight,
        Sleeping
    }
}
