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
        public string ImagePath { get; set; }

        private Thread thread = null;

        public CatActuator actuator { get; set; }

        private Random random = new Random();

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
            int timeToNextCat = random.Next(8000, 30001);

            Thread.Sleep(timeToNextCat);

            actuator.TriggerShowCat(true);

            Thread.Sleep(Config.environmentCatShowingDelay);

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
