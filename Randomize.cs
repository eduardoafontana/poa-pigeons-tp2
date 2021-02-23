using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// Auxiliary class for generating random numbers.
    /// Randomization needs to consider the closed interval, so it adds up to 1 at the end.
    /// The Next default method considers an open interval.
    /// </summary>
    public static class Randomize
    {
        private static Random random = new Random();

        public static int GetValue(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }
    }
}
