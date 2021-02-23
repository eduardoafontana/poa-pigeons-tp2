using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is the actuator of the cat. 
    /// The actuator is the thread device that has delegates that can be linked to other classes.
    /// That is, whenever a class wants to receive a notification from this thread, it must sign one of the actuator's delegates.
    /// Between threads, the actuators of thread A are signed by the sensor of thread B. 
    /// In this way, the threads communicate through events of asynchronous delegates.
    /// </summary>
    public class CatActuator
    {
        public delegate void ShowingCatActuator(bool appear);
        public event ShowingCatActuator RaiseShowCat;

        public CatActuator()
        {
        }

        public void TriggerShowCat(bool appear)
        {
            RaiseShowCat(appear);
        }
    }
}
