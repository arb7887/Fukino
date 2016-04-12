using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatGame
{
    class Bullet
    {
        enum Alignment
        {
            Player,
            Enemy
        }
        double speed, damage, size;

        public bool IsColliding(Unit u)
        {
            return false;
        }

        public bool IsColliding()
        {
            return false;
        }
    }
}
