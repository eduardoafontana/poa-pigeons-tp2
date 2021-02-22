using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Cat : IElement
    {
        public string ImagePath { get; }

        private Thread thread = null;

        public CatActuator actuator { get; set; }

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
