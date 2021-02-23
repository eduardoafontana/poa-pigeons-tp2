using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class represents the cat character, who eventually appears to scare the pigeons away.
    /// The class is responsible for controlling the animation of the cat and its appearance.
    /// All behavior is controlled via a thread. 
    /// Interactions between threads are performed by triggering an event from the actuator. Just as the events of other threads are perceived through the sensors.
    /// </summary>
    public class Cat : IElement
    {
        public string ImagePath { get; }

        private Thread thread = null;

        public CatActuator actuator { get; }

        public Cat()
        {
            ImagePath = "Assets\\cat.png";

            actuator = new CatActuator();

            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        private void Loop()
        {
            while (true)
            {
                Execute();
            }
        }

        internal void Execute()
        {
            int timeToNextCat = Randomize.GetValue(Config.catMinTimeToNextCat, Config.catMaxTimeToNextCat);

            Thread.Sleep(timeToNextCat);

            actuator.TriggerShowCat(true);

            Thread.Sleep(Config.catShowingDelay);

            actuator.TriggerShowCat(false);
        }

        public void Destroy()
        {
            if (thread != null)
                thread.Abort();
        }

        ~Cat()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
