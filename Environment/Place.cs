using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Place
    {
        public Pigeon Pigeon { get; set; }
        public Food Food { get; set; }
        public int Index { get; }
        public PlaceSensor Sensor { get; }

        public Place(Environment environment, int index)
        {
            Index = index;
            Sensor = new PlaceSensor(environment, this);
        }

        internal bool isClean()
        {
            return Pigeon == null && Food == null;
        }
    }
}
