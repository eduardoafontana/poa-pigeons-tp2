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

        private PigeonAction pigeonAction;

        private Thread thread = null;

        public PigeonActuator actuator { get; set; }

        private Random random = new Random();

        private int index = 0;

        private List<Place> places;

        private int currentPosition;

        private int timesWaiting = 0;

        private int maxTimeWiting;

        public PigeonSensor sensor;

        public Pigeon(List<Place> places, int currentPosition)
        {
            index = random.Next(1, 10);
            maxTimeWiting = random.Next(Config.pigeonMinTimesWaiting, Config.pigeonMaxTimesWaiting);

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
            int indexLeftRight = random.Next(0, 100);

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

        private bool ExecuteTryEatFood(int tryingPosition, int foodPosition)
        {
            if (places[tryingPosition].food != null && places[tryingPosition].food.CurrentState == FoodState.Good)
            {
                SetDirection(tryingPosition);
                EatFood(tryingPosition);

                return true;
            }

            if (places[tryingPosition].pigeon != null)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            if (places[foodPosition].food == null)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            if (places[foodPosition].food != null && places[foodPosition].food.CurrentState != FoodState.Good)
            {
                pigeonAction = PigeonAction.Waiting;
                return true;
            }

            SetDirection(tryingPosition);
            ExecuteWalk(tryingPosition);

            return false;
        }

        private int VerifyIfThereIsFood()
        {
            int firstPositionLeft = currentPosition - 1;

            for (int i = firstPositionLeft; i >= 0; i--)
            {
                if (places[i].food != null && places[i].food.CurrentState == FoodState.Good)
                    return i;
            }

            int firstPositionRight = currentPosition + 1;

            for (int i = firstPositionRight; i < Config.environmentSize; i++)
            {
                if (places[i].food != null && places[i].food.CurrentState == FoodState.Good)
                    return i;
            }

            return -1;
        }

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

        private void SetSleep()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_Zzzz.png", index);
            pigeonAction = PigeonAction.Sleeping;

            timesWaiting = 0;

            actuator.TriggerChangePigeon();
        }

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

        private void ExecuteWalk(int newPosition)
        {
            places[newPosition].pigeon = places[currentPosition].pigeon;
            places[currentPosition].pigeon = null;
            currentPosition = newPosition;

            actuator.TriggerChangePigeon();

            places[newPosition].sensor.AddPigeonInPlace(places[newPosition].pigeon);
            actuator.TriggerChangePigeon();

            Thread.Sleep(Config.pigeonActionDelay);

            pigeonAction = PigeonAction.Waiting;
        }

        internal void WakeUpOrChangePosition()
        {
            if (pigeonAction == PigeonAction.Sleeping)
            {
                SetWakeUp();
                return;
            }

            int newPosition = random.Next(0, Config.environmentSize);

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
