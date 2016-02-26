using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSGame
{
    interface ICollidable
    {
        bool IsColliding(Unit u);
        bool IsColliding(Terrain t);
    }
}
