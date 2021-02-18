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
        private Cat cat;

        public EnvironmentActuator actuator = new EnvironmentActuator();

        public Environment()
        {
            for (int i = 0; i < Config.environmentSize; i++)
            {
                places.Add(new Place(this, i));
            }

            cat = new Cat();
        }

        internal void Execute()
        {
            if (ShouldThereBeANewPigeon())
                GeneratePigeon();
        }

        private void GeneratePigeon()
        {
            Place place = places[currentRandomPosition];
            place.pigeon = new Pigeon(places, currentRandomPosition);
            place.pigeon.sensor.AddCatAffraid(cat);
            place.sensor.AddPigeonInPlace(place.pigeon);
            place.sensor.AddCatBehavior(cat);

            actuator.TriggerChangeEnvironment(place);

            Thread.Sleep(Config.environmentPigeonDelay);
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
        
            if(cat != null)
            {
                cat.Destroy();
                cat = null;
            }
        }
    }
}
