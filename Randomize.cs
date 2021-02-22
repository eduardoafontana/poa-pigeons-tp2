using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public static class Randomize
    {
        private static Random random = new Random();

        public static int GetValue(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }
    }
}
