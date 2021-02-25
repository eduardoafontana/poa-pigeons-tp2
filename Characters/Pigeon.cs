using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This class represents the pigeon character. 
    /// The pigeon awaits food. If he finds no food, he sleeps.
    /// If he find fresh food, he walks towards the food. If he can get to the food before another pigeon and the food is still fresh, he eats.
    /// If the pigeon is sleeping and the cat appears, the pigeon wakes up.
    /// If the pigeon has woken up and the cat appears, it disperses to another random position.
    /// The class is responsible for controlling the animation of the pigeon and the interaction between the pigeon and the other characters.
    /// All behavior is controlled via a thread. 
    /// Interactions between threads are performed by triggering an event from the actuator. Just as the events of other threads are perceived through the sensors.
    /// </summary>
    public class Pigeon : ICharacter
    {
        public string ImagePath { get; private set; }
        public PigeonActuator actuator { get; }
        public PigeonSensor sensor { get; }

        private PigeonAction pigeonAction;
        private Thread thread = null;
        private int index = 0;
        private List<Place> places;
        private int currentPosition;
        private int timesWaiting = 0;
        private int maxTimeWiting;

        public Pigeon(List<Place> places, int currentPosition)
        {
            index = Randomize.GetValue(1, 9);
            maxTimeWiting = Randomize.GetValue(Config.pigeonMinTimesWaiting, Config.pigeonMaxTimesWaiting);

            ChoseDirection();

            pigeonAction = PigeonAction.Waiting;

            actuator = new PigeonActuator();
            sensor = new PigeonSensor(this);

            this.places = places;
            this.currentPosition = currentPosition;

            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        private void ChoseDirection()
        {
            int indexLeftRight = Randomize.GetValue(0, 100);

            if (indexLeftRight % 2 == 0)
                SetDirectionLeft();
            else
                SetDirectionRight();
        }

        private void SetDirectionRight()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_right.png", index);
        }

        private void SetDirectionLeft()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_left.png", index);
        }

        private void Loop()
        {
            while (true)
            {
                Execute();
            }
        }

        /// <summary>
        /// This method contains the main logic of the pigeon's behavior.
        /// The pigeon looks for food. If he finds no food, he sleeps.
        /// </summary>
        internal void Execute()
        {
            if (pigeonAction == PigeonAction.Sleeping)
                Thread.Sleep(Config.pigeonSleepDelay);

            Thread.Sleep(Config.pigeonActionDelay);

            if (pigeonAction != PigeonAction.Waiting)
                return;

            bool timeToSleep;

            int foodPosition = VerifyIfThereIsFood();

            if (foodPosition > 0)
            {
                TryEatFood(foodPosition);
                return;
            }

            timesWaiting++;

            timeToSleep = VerifyIfItIsTimeToSleep();

            if (timeToSleep)
            {
                SetSleep();
                return;
            }
        }

        /// <summary>
        /// This method is responsible for making the pigeon move to the food found.
        /// </summary>
        private void TryEatFood(int foodPosition)
        {
            if (foodPosition > currentPosition)
            {
                int firstPositionRight = currentPosition + 1;

                for (int i = firstPositionRight; i < Config.environmentSize; i++)
                {
                    bool stopGoForward = ExecuteTryEatFood(i, foodPosition);

                    if (stopGoForward)
                        return;
                }
            }
            else
            {
                int firstPositionLeft = currentPosition - 1;

                for (int i = firstPositionLeft; i >= 0; i--)
                {
                    bool stopGoForward = ExecuteTryEatFood(i, foodPosition);

                    if (stopGoForward)
                        return;
                }
            }
        }

        /// <summary>
        /// As pigeon move until the food found to eat, other events can happen.
        /// Another pigeon may appear in the front or another food may appear in the front or the target food will rot.
        /// So, this method is responsible for making the pigeon notice the state of the next place he will step on, to know if he will change his behavior or not.
        /// </summary>
        private bool ExecuteTryEatFood(int tryingPosition, int foodPosition)
        {
            if (places[tryingPosition].Food != null && places[tryingPosition].Food.CurrentState == FoodState.Good)
            {
                SetDirection(tryingPosition);
                EatFood(tryingPosition);

                return true;
            }

            if (places[tryingPosition].Pigeon != null)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            if (places[foodPosition].Food == null)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            if (places[foodPosition].Food != null && places[foodPosition].Food.CurrentState != FoodState.Good)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            SetDirection(tryingPosition);
            ExecuteWalk(tryingPosition);

            return false;
        }

        /// <summary>
        /// This method is responsible for making the pigeon look around the environment to find food.
        /// </summary>
        private int VerifyIfThereIsFood()
        {
            int firstPositionLeft = currentPosition - 1;

            for (int i = firstPositionLeft; i >= 0; i--)
            {
                if (places[i].Food != null && places[i].Food.CurrentState == FoodState.Good)
                    return i;
            }

            int firstPositionRight = currentPosition + 1;

            for (int i = firstPositionRight; i < Config.environmentSize; i++)
            {
                if (places[i].Food != null && places[i].Food.CurrentState == FoodState.Good)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// This method is responsible for performing the act of eating the pigeon.
        /// Note that eating the food requires interacting with the environment and with the food. 
        /// This interaction is not done here, but delegated through the actuator.
        /// In other words, the Food and Environment classes that listen to the pigeon thread will know when it ate and perform the appropriate action.
        /// </summary>
        private void EatFood(int newPosition)
        {
            String currentDirection = ImagePath;

            if(ImagePath.Contains("left"))
                ImagePath = String.Format("Assets\\pigeon{0}_left_nham.png", index);
            else
                ImagePath = String.Format("Assets\\pigeon{0}_right_nham.png", index);

            actuator.TriggerEatPigeon(newPosition);
            actuator.TriggerChangePigeon();

            Thread.Sleep(Config.pigeonActionDelay);

            ImagePath = currentDirection;

            actuator.TriggerChangePigeon();

            pigeonAction = PigeonAction.Waiting;
        }

        /// <summary>
        /// This method is responsible for putting the pigeon to sleep.
        /// </summary>
        private void SetSleep()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_Zzzz.png", index);
            pigeonAction = PigeonAction.Sleeping;

            timesWaiting = 0;

            actuator.TriggerChangePigeon();
        }

        /// <summary>
        /// This method is responsible for waking up the pigeon.
        /// </summary>
        private void SetWakeUp()
        {
            pigeonAction = PigeonAction.Waiting;

            ChoseDirection();

            actuator.TriggerChangePigeon();
        }

        private bool VerifyIfItIsTimeToSleep()
        {
            return timesWaiting >= maxTimeWiting;
        }

        /// <summary>
        /// This method is responsible for changing the position of the pigeon.
        /// </summary>
        private void ExecuteWalk(int newPosition)
        {
            places[newPosition].Pigeon = places[currentPosition].Pigeon;
            places[currentPosition].Pigeon = null;
            currentPosition = newPosition;

            actuator.TriggerChangePigeon();

            places[newPosition].Sensor.AddPigeonInPlace(places[newPosition].Pigeon);
            actuator.TriggerChangePigeon();

            Thread.Sleep(Config.pigeonActionDelay);

            pigeonAction = PigeonAction.Waiting;
        }

        /// <summary>
        /// This method is triggered when the cat appears.
        /// The pigeon must wake up or disperse to a random position.
        /// </summary>
        internal void WakeUpOrChangePosition()
        {
            if (pigeonAction == PigeonAction.Sleeping)
            {
                SetWakeUp();
                return;
            }

            int newPosition = Randomize.GetValue(0, Config.environmentSize - 1);

            if (!places[newPosition].isClean())
                return;

            SetDirection(newPosition);

            ExecuteWalk(newPosition);
        }

        private void SetDirection(int newPosition)
        {
            if (newPosition > currentPosition)
            {
                SetDirectionRight();
                pigeonAction = PigeonAction.WalkingRight;
            }
            else
            {
                SetDirectionLeft();
                pigeonAction = PigeonAction.WalkingLeft;
            }
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
