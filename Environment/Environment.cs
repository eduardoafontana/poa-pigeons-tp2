using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Environment
    {
        private List<Place> places = new List<Place>();
        private Random random = new Random();
        private int currentRandomPosition;

        public EnvironmentSensor sensor;
        public EnvironmentActuator actuator = new EnvironmentActuator();

        public Environment()
        {
            for (int i = 0; i < Config.environmentSize; i++)
            {
                places.Add(new Place());
            }

            sensor = new EnvironmentSensor(this);
        }

        internal void Execute()
        {
            Thread.Sleep(Config.environmentActionDelay);

            if (ShouldThereBeANewPigeon())
                GeneratePigeon();

            //other events triggered by environment
        }

        private void GeneratePigeon()
        {
            places[currentRandomPosition].pigeon = new Pigeon();

            actuator.TriggerChangeEnvironment(places);
        }

        private bool ShouldThereBeANewPigeon()
        {
            currentRandomPosition = random.Next(0, Config.environmentSize);

            if (places[currentRandomPosition].pigeon == null)
                return true;

            return false;
        }

        internal void CloseThreads()
        {
            foreach (var item in places)
            {
                if (item.pigeon != null)
                {
                    item.pigeon.Destroy();
                    item.pigeon = null;
                }
            }
        }
    }
}
