using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is a configuration class. 
    /// It is responsible for storing all fixed values that are used to guide the game's behavior. 
    /// If in the future we want to open up to the user to configure the game's behavior, just throw the values of that class into a form.
    /// </summary>
    public class Config
    {
        public static int environmentSize = 19;
        public static int environmentPlaceSizeWidth = 40;
        public static int environmentPlaceSizeHeight = 100;

        public static int environmentPigeonDelay = 6000;
        public static int environmentMaxNumberOfPigeons = 5;

        public static int pigeonActionDelay = 1000;
        public static int pigeonSleepDelay = 2000;
        public static int pigeonMinTimesWaiting = 10;
        public static int pigeonMaxTimesWaiting = 30;

        public static int foodMinTimeChangeState = 5000;
        public static int foodMaxTimeChangeState = 12000;

        public static int catMinTimeToNextCat = 8000;
        public static int catMaxTimeToNextCat = 30000;
        public static int catShowingDelay = 3500;
    }
}
