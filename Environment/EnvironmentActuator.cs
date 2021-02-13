using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class EnvironmentActuator
    {
        public delegate void ChangingEnvironmentActuator(List<Place> places);
        public event ChangingEnvironmentActuator RaiseChangeEnvironment;

        public EnvironmentActuator()
        {
        }

        public void TriggerChangeEnvironment(List<Place> places)
        {
            RaiseChangeEnvironment(places);
        }
    }
}
