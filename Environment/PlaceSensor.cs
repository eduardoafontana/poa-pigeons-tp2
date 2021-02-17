using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class PlaceSensor
    {
        private Environment environment;
        private Place place;

        public PlaceSensor(Environment environment, Place place)
        {
            this.environment = environment;
            this.place = place;
        }

        internal void AddPigeonInPlace(IElement pigeon)
        {
            pigeon.actuator.RaiseChangePigeon += new PigeonActuator.ChangingPigeonActuator(placeSensor_OnPigeonChange);
        }

        private void placeSensor_OnPigeonChange()
        {
            environment.Change(place);
        }
    }
}
