using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSGame
{
    class Terrain : ICollidable
    {
        public bool IsColliding(Unit u)
        {
            return false;
        }

        public bool IsColliding(Terrain t)
        {
            return false;
        }
    }
}
