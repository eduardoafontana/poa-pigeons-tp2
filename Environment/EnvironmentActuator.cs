using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
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
