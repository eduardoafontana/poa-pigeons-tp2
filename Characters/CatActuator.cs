using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public class CatActuator
    {
        public delegate void ShowingCatActuator(bool appear);
        public event ShowingCatActuator RaiseShowCat;

        public CatActuator()
        {
        }

        public void TriggerShowCat(bool appear)
        {
            RaiseShowCat(appear);
        }
    }
}
