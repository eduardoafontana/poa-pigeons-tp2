using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Config
    {
        public static int environmentSize = 19;
        public static int environmentPlaceSizeWidth = 40;
        public static int environmentPlaceSizeHeight = 100;
        public static int elementSize = 15;

        public static int environmentPigeonDelay = 6000;
        public static int environmentCatShowingDelay = 5000;
        public static int pigeonActionDelay = 2000;

        public static int pigeonMaxTimesTurn = 3;
        public static int pigeonMaxTimesWalk = 5;
    }
}
