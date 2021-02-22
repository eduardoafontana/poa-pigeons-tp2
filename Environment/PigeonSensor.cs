using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
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
