using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
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
