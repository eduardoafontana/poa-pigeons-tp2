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

        public PigeonActuator actuator { get; set; }

        private Random random = new Random();

        private int index = 0;

        public Pigeon()
        {
            index = random.Next(1, 1); //TODO after, alter this second 1 for the number of pigeons images

            GenerateImage();

            thread = new Thread(new ThreadStart(Loop));
            thread.Start();

            actuator = new PigeonActuator();
        }

        private void GenerateImage()
        {
            int indexLeftRight = random.Next(0, 100);

            if (indexLeftRight % 2 == 0)
                ImagePath = String.Format("Assets\\pigeon{0}_left.png", index);
            else
                ImagePath = String.Format("Assets\\pigeon{0}_right.png", index);
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
            Thread.Sleep(Config.pigeonActionDelay);

            GenerateImage();

            actuator.TriggerTurnPigeon();
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
