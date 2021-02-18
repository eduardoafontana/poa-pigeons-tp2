using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Place
    {
        public Pigeon pigeon { get; set; }
        //public IElement food { get; set; }

        public int index { get; set; }

        public PlaceSensor sensor;

        public Place(Environment environment, int index)
        {
            this.index = index;
            sensor = new PlaceSensor(environment, this);
        }

        internal bool isClean()
        {
            //return pigeon == null && food == null;
            return pigeon == null;
        }
    }
}
