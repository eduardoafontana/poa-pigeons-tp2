using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class PigeonActuator
    {
        public delegate void TurningPigeonActuator();
        public event TurningPigeonActuator RaiseTurnPigeon;

        public PigeonActuator()
        {
        }

        public void TriggerTurnPigeon()
        {
            RaiseTurnPigeon();
        }
    }
}
