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
        private int timesWalkAfterTurn = 0;

        public Pigeon(List<Place> places, int currentPosition)
        {
            index = random.Next(1, 10);

            ChoseDirection();

            actuator = new PigeonActuator();

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

            switch (pigeonAction)
            {
                case PigeonAction.WalkingLeft:
                    timeToSleep = VerifyIfItIsTimeToSleep();

                    if (timeToSleep)
                    {
                        SetSleep();
                        break;
                    }

                    newPosition = currentPosition - 1;

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
                    timeToSleep = VerifyIfItIsTimeToSleep();

                    newPosition = currentPosition + 1;

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

        private void SetSleep()
        {
            ImagePath = String.Format("Assets\\pigeon{0}_Zzzz.png", index);
            pigeonAction = PigeonAction.Sleeping;

            timesTurn = 0;
            timesWalkAfterTurn = 0;

            actuator.TriggerChangePigeon();
        }

        private bool VerifyIfItIsTimeToSleep()
        {
            return timesTurn >= Config.pigeonMaxTimesTurn && timesWalkAfterTurn >= Config.pigeonMaxTimesWalkAfterTurn;
        }

        private void ExecuteWalk(int newPosition)
        {
            places[newPosition].pigeon = places[currentPosition].pigeon;
            places[currentPosition].pigeon = null;
            currentPosition = newPosition;

            actuator.TriggerChangePigeon();

            places[newPosition].sensor.AddPigeonInPlace(places[newPosition].pigeon);
            actuator.TriggerChangePigeon();

            if (timesTurn >= Config.pigeonMaxTimesTurn)
                timesWalkAfterTurn++;
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
