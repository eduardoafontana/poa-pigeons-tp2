using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class represents the window. It is responsible for controlling the addition of the pigeons and making the cat appear.
    /// In addition, it is responsible for adding food to the environment when the user clicks on the window.
    /// In addition, it is responsible for feeding the pigeon, because when the pigeon feeds, it interacts with the environment.
    /// </summary>
    public class Environment
    {
        private List<Place> places = new List<Place>();
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
                place.Sensor.AddCatBehavior(cat);

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

            place.Pigeon = new Pigeon(places, currentRandomPosition);
            place.Pigeon.sensor.AddCatAffraid(cat);

            place.Sensor.AddPigeonInPlace(place.Pigeon);

            actuator.TriggerChangeEnvironment(place);

            quantityOfPigeons++;

            Thread.Sleep(Config.environmentPigeonDelay);
        }

        internal void ExecuteEat(int position)
        {
            if (places[position].Food == null)
                return;

            if (places[position].Food.CurrentState != FoodState.Good)
                return;

            places[position].Food.ExecuteWasEaten();
        }

        private bool ShouldThereBeANewPigeon()
        {
            if (quantityOfPigeons >= Config.environmentMaxNumberOfPigeons)
                return false;

            currentRandomPosition = Randomize.GetValue(0, Config.environmentSize - 1);

            if (places[currentRandomPosition].Pigeon == null)
                return true;

            return false;
        }

        internal void TryAddFood(int position)
        {
            if (!places[position].isClean())
                return;

            Place place = places[position];
            place.Food = new Food(places, position);
            place.Sensor.AddFoodInPlace(place.Food);

            actuator.TriggerChangeEnvironment(place);
        }

        internal void CloseThreads()
        {
            foreach (var item in places)
            {
                if (item.Pigeon != null)
                {
                    item.Pigeon.Destroy();
                    item.Pigeon = null;
                }

                if (item.Food != null)
                {
                    item.Food.Destroy();
                    item.Food = null;
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
