using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Pigeon : IElement
    {
        public string ImagePath { get; set; }

        private Thread thread = null;

        public Pigeon()
        {
            Random random = new Random();

            int index = random.Next(1, 1);

            //ImagePath = String.Format("Assets\\pigeon{0}_right.png", index);
            ImagePath = String.Format("Assets\\pigeon1.png", index);

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
            Thread.Sleep(Config.environmentActionDelay);

            Debug.WriteLine(DateTime.Now);
        }

        public void Destroy()
        {
            if (thread != null)
                thread.Abort();
        }

        ~Pigeon()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
