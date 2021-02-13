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

        public EnvironmentActuator actuator = new EnvironmentActuator();

        public Environment()
        {
            for (int i = 0; i < Config.environmentSize; i++)
            {
                places.Add(new Place(this, i));
            }
        }

        internal void Change(Place place)
        {
            actuator.TriggerChangeEnvironment(place);
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
            Place place = places[currentRandomPosition];
            place.pigeon = new Pigeon();
            place.sensor.AddPigeonInPlace(place.pigeon);

            actuator.TriggerChangeEnvironment(place);
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
