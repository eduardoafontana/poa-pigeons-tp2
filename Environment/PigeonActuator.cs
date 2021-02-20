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

        public delegate void EatingPigeonActuator(int position);
        public event EatingPigeonActuator RaiseEatPigeon;

        public PigeonActuator()
        {
        }

        public void TriggerChangePigeon()
        {
            RaiseChangePigeon();
        }

        public void TriggerEatPigeon(int position)
        {
            RaiseEatPigeon(position);
        }
    }
}
