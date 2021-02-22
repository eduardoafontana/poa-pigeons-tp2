using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class Food : IElement
    {
        public string ImagePath { get; set; }

        private Thread thread = null;

        public FoodActuator actuator { get; set; }

        private int position;

        private FoodState currentState;

        public FoodState CurrentState { get { return currentState; } }

        private List<Place> places;

        private Random random = new Random();

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
            int mileseconds = random.Next(Config.foodMinTimeChangeState, Config.foodMaxTimeChangeState);

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
                    places[position].food = null;
                    actuator.TriggerChangeFood();
                    Destroy();
                    break;
            }
        }

        internal void ExecuteWasEaten()
        {
            places[position].food = null;
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
