using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is the actuator of the food. 
    /// The actuator is the thread device that has delegates that can be linked to other classes.
    /// That is, whenever a class wants to receive a notification from this thread, it must sign one of the actuator's delegates.
    /// Between threads, the actuators of thread A are signed by the sensor of thread B. 
    /// In this way, the threads communicate through events of asynchronous delegates.
    /// </summary>
    public class FoodActuator
    {
        public delegate void ChangingFoodActuator();
        public event ChangingFoodActuator RaiseChangeFood;

        public FoodActuator()
        {
        }

        public void TriggerChangeFood()
        {
            RaiseChangeFood();
        }
    }
}
