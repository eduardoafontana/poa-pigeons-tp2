using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// This is the engine of the game. 
    /// This class is responsible for creating the environment, which is represented by the window, and its respective thread. 
    /// In addition, this class takes care of closing threads when the form is closed.
    /// </summary>
    public class Engine
    {
        private static Thread environmentThread = null;

        public static Environment environment;

        internal static void Init()
        {
            environment = new Environment();

            environmentThread = new Thread(new ThreadStart(EnvironmentLoop));
        }

        internal static void Start()
        {
            environmentThread.Start();
        }

        private static void EnvironmentLoop()
        {
            while (true)
            {
                environment.Execute();
            }
        }

        internal static void Stop()
        {
            if (environmentThread != null)
                environmentThread.Abort();

            environment.CloseThreads();
        }
    }
}
