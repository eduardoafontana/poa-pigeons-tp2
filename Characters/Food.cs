using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class represents the food character. 
    /// The food can be eaten by the pigeon. If it is not eaten, it will rot and disappear.
    /// The class is responsible for controlling the animation of the food.
    /// All behavior is controlled via a thread. 
    /// Interactions between threads are performed by triggering an event from the actuator. Just as the events of other threads are perceived through the sensors.
    /// </summary>
    public class Food : ICharacter
    {
        public string ImagePath { get; private set; }
        public FoodActuator actuator { get; }
        public FoodState CurrentState { get { return currentState; } }

        private Thread thread = null;
        private int position;
        private FoodState currentState;
        private List<Place> places;

        public Food(List<Place> places, int position)
        {
            actuator = new FoodActuator();

            this.places = places;
            this.position = position;

            ChangeState(FoodState.Good);

            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        private void ChangeState(FoodState state)
        {
            currentState = state;
            ImagePath = String.Format("Assets\\food_{0}.png", currentState.ToString().ToLower());
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
            int mileseconds = Randomize.GetValue(Config.foodMinTimeChangeState, Config.foodMaxTimeChangeState);

            Thread.Sleep(mileseconds);

            switch (currentState)
            {
                case FoodState.Good:
                    ChangeState(FoodState.Medium);
                    actuator.TriggerChangeFood();
                    break;
                case FoodState.Medium:
                    ChangeState(FoodState.Rotten);
                    actuator.TriggerChangeFood();
                    break;
                case FoodState.Rotten:
                    places[position].Food = null;
                    actuator.TriggerChangeFood();
                    Destroy();
                    break;
            }
        }

        internal void ExecuteWasEaten()
        {
            places[position].Food = null;
            actuator.TriggerChangeFood();
            Destroy();
        }

        public void Destroy()
        {
            if (thread != null)
                thread.Abort();
        }

        ~Food()
        {
            if (thread != null)
                thread.Abort();
        }
    }
}
