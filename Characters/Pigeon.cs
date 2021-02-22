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
            places[newPosition].Pigeon = places[currentPosition].Pigeon;
            places[currentPosition].Pigeon = null;
            currentPosition = newPosition;

            actuator.TriggerChangePigeon();

            places[newPosition].Sensor.AddPigeonInPlace(places[newPosition].Pigeon);
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
