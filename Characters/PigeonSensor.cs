using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is the pigeon sensor.
    /// The sensor is an auxiliary class to the main entity.
    /// It is responsible for containing the methods that associate an actuator of some thread to which the main class wants to communicate asynchronously.
    /// Upon receiving the actuator event, the sensor can call a main class method to execute some behavior or override a behavior by delegating to the main class actuator.
    /// </summary>
    public class PigeonSensor
    {
        private Pigeon pigeon;

        public PigeonSensor(Pigeon pigeon)
        {
            this.pigeon = pigeon;
        }

        internal void AddCatAffraid(Cat cat)
        {
            cat.actuator.RaiseShowCat += new CatActuator.ShowingCatActuator(pigeonSensor_OnCatShow);
        }

        private void pigeonSensor_OnCatShow(bool appear)
        {
            if (!appear)
                return;

            pigeon.WakeUpOrChangePosition();
        }
    }
}
