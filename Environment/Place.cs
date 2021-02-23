using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class represents a piece of the environment that interacts with the character.
    /// As we are using windows forms, animating the graphic elements is expensive and complex.
    /// So, to make it easier, we use a table panel that divides the form into several sections.
    /// Each section is called a place and can be manipulated individually.
    /// Each place can contain either a pigeon or food.
    /// </summary>
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
