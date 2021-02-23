using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is the place sensor.
    /// The sensor is an auxiliary class to the main entity.
    /// It is responsible for containing the methods that associate an actuator of some thread to which the main class wants to communicate asynchronously.
    /// Upon receiving the actuator event, the sensor can call a main class method to execute some behavior or override a behavior by delegating to the main class actuator.
    /// </summary>
    public class PlaceSensor
    {
        private Environment environment;
        private Place place;

        public PlaceSensor(Environment environment, Place place)
        {
            this.environment = environment;
            this.place = place;
        }

        internal void AddPigeonInPlace(Pigeon pigeon)
        {
            if (pigeon == null)
                return;

            pigeon.actuator.RaiseChangePigeon += new PigeonActuator.ChangingPigeonActuator(placeSensor_OnPigeonChange);
            pigeon.actuator.RaiseEatPigeon += new PigeonActuator.EatingPigeonActuator(placeSensor_OnPigeonEat);
        }

        internal void AddFoodInPlace(Food food)
        {
            if (food == null)
                return;

            food.actuator.RaiseChangeFood += new FoodActuator.ChangingFoodActuator(placeSensor_OnFoodChange);
        }

        private void placeSensor_OnPigeonChange()
        {
            environment.actuator.TriggerChangeEnvironment(place);
        }

        private void placeSensor_OnPigeonEat(int position)
        {
            environment.ExecuteEat(position);
        }

        private void placeSensor_OnFoodChange()
        {
            environment.actuator.TriggerChangeEnvironment(place);
        }

        internal void AddCatBehavior(Cat cat)
        {
            cat.actuator.RaiseShowCat += new CatActuator.ShowingCatActuator(placeSensor_OnCatShow);
        }

        private void placeSensor_OnCatShow(bool appear)
        {
            environment.actuator.TriggerChangeCat(appear);
        }
    }
}
