using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class PigeonActuator
    {
        public delegate void ChangingPigeonActuator();
        public event ChangingPigeonActuator RaiseChangePigeon;

        public PigeonActuator()
        {
        }

        public void TriggerChangePigeon()
        {
            RaiseChangePigeon();
        }
    }
}
