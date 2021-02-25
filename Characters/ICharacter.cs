using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PigeonsTP2
{
    /// <summary>
    /// Interface that determines the minimum behavior that a character element has to have.
    /// </summary>
    public interface ICharacter
    {
        string ImagePath { get; }

        void Destroy();
    }
}
