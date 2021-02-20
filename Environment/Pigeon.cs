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

        private int timesTurn = -1;

        private int timesWalk = 0;

        public PigeonSensor sensor;

        public Pigeon(List<Place> places, int currentPosition)
        {
            index = random.Next(1, 10);

            ChoseDirection();

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
            pigeonAction = PigeonAction.WalkingRight;

            timesTurn++;
        }

        private void SetDirectionLeft()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_left.png", index);
            pigeonAction = PigeonAction.WalkingLeft;

            timesTurn++;
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
            Thread.Sleep(Config.pigeonActionDelay);

            int newPosition;
            bool timeToSleep;
            bool thereIsFood;

            switch (pigeonAction)
            {
                case PigeonAction.WalkingLeft:
                    newPosition = currentPosition - 1;

                    thereIsFood = VerifyIfThereIsFoodLeft(newPosition);

                    if (thereIsFood)
                    {
                        EatFood(newPosition);
                        break;
                    }

                    timeToSleep = VerifyIfItIsTimeToSleep();

                    if (timeToSleep)
                    {
                        SetSleep();
                        break;
                    }

                    if (newPosition < 0)
                    {
                        SetDirectionRight();
                        break;
                    }

                    if (!places[newPosition].isClean())
                    {
                        SetDirectionRight();
                        break;
                    }

                    ExecuteWalk(newPosition);

                    break;
                case PigeonAction.WalkingRight:
                    newPosition = currentPosition + 1;

                    thereIsFood = VerifyIfThereIsFoodRight(newPosition);

                    if (thereIsFood)
                    {
                        EatFood(newPosition);
                        break;
                    }

                    timeToSleep = VerifyIfItIsTimeToSleep();

                    if (timeToSleep)
                    {
                        SetSleep();
                        break;
                    }

                    if (newPosition == places.Count)
                    {
                        SetDirectionLeft();
                        break;
                    }

                    if (!places[newPosition].isClean())
                    {
                        SetDirectionLeft();
                        break;
                    }

                    ExecuteWalk(newPosition);

                    break;
                case PigeonAction.Sleeping:
                    break;
                case PigeonAction.Eating:
                    break;
            }
        }

        private void EatFood(int newPosition)
        {
            actuator.TriggerEatPigeon(newPosition);

            Thread.Sleep(1000);
            //Aqui fazer a lógica de alteração da imagem talvez
        }

        private bool VerifyIfThereIsFoodLeft(int newPosition)
        {
            if (newPosition < 0)
                return false;

            return places[newPosition].food != null;
        }

        private bool VerifyIfThereIsFoodRight(int newPosition)
        {
            if (newPosition == places.Count)
                return false;

            return places[newPosition].food != null;
        }

        private void SetSleep()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_Zzzz.png", index);
            pigeonAction = PigeonAction.Sleeping;

            timesTurn = 0;
            timesWalk = 0;

            actuator.TriggerChangePigeon();
        }

        private void SetWakeUp()
        {
            ChoseDirection();

            actuator.TriggerChangePigeon();
        }

        private bool VerifyIfItIsTimeToSleep()
        {
            return timesTurn >= Config.pigeonMaxTimesTurn || timesWalk >= Config.pigeonMaxTimesWalk;
        }

        private void ExecuteWalk(int newPosition)
        {
            places[newPosition].pigeon = places[currentPosition].pigeon;
            places[currentPosition].pigeon = null;
            currentPosition = newPosition;

            actuator.TriggerChangePigeon();

            places[newPosition].sensor.AddPigeonInPlace(places[newPosition].pigeon);
            actuator.TriggerChangePigeon();

            timesWalk++;
        }

        internal void Fly()
        {
            if (pigeonAction == PigeonAction.Sleeping)
            {
                SetWakeUp();
                return;
            }

            places[currentPosition].pigeon = null;
            actuator.TriggerChangePigeon();

            Destroy();
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
