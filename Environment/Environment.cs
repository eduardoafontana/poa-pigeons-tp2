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
        private int quantityOfPigeons = 0;

        public EnvironmentActuator actuator = new EnvironmentActuator();

        public Environment()
        {
            cat = new Cat();

            for (int i = 0; i < Config.environmentSize; i++)
            {
                Place place = new Place(this, i);
                place.sensor.AddCatBehavior(cat);

                places.Add(place);
            }
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

            actuator.TriggerChangeEnvironment(place);

            quantityOfPigeons++;

            Thread.Sleep(Config.environmentPigeonDelay);
        }

        internal void ExecuteEat(int position)
        {
            if (places[position].food == null)
                return;

            if (places[position].food.CurrentState != FoodState.Good)
                return;

            places[position].food.ExecuteWasEaten();
        }

        private bool ShouldThereBeANewPigeon()
        {
            if (quantityOfPigeons >= Config.environmentMaxNumberOfPigeons)
                return false;

            currentRandomPosition = random.Next(0, Config.environmentSize);

            if (places[currentRandomPosition].pigeon == null)
                return true;

            return false;
        }

        internal void TryAddFood(int position)
        {
            if (!places[position].isClean())
                return;

            Place place = places[position];
            place.food = new Food(places, position);
            place.sensor.AddFoodInPlace(place.food);

            actuator.TriggerChangeEnvironment(place);
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

                if (item.food != null)
                {
                    item.food.Destroy();
                    item.food = null;
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
