using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is the actuator of the environment. 
    /// The actuator is the thread device that has delegates that can be linked to other classes.
    /// That is, whenever a class wants to receive a notification from this thread, it must sign one of the actuator's delegates.
    /// Between threads, the actuators of thread A are signed by the sensor of thread B. 
    /// In this way, the threads communicate through events of asynchronous delegates.
    /// </summary>
    public class EnvironmentActuator
    {
        public delegate void ChangingEnvironmentActuator(Place place);
        public event ChangingEnvironmentActuator RaiseChangeEnvironment;

        public delegate void ChangingCatActuator(bool visible);
        public event ChangingCatActuator RaiseChangeCat;

        public EnvironmentActuator()
        {
        }

        public void TriggerChangeEnvironment(Place place)
        {
            RaiseChangeEnvironment(place);
        }

        public void TriggerChangeCat(bool visible)
        {
            RaiseChangeCat(visible);
        }
    }
}
