using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    public interface IElement
    {
        string ImagePath { get; set; }

        void Destroy();
    }
}
