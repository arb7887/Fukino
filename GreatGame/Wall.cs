using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Wall
    {
        private BoundingBox bounds;
        public BoundingBox Bounds { get { return bounds; } }

        public Wall(BoundingBox bound)
        {
            bounds = bound;
        }
    }
}
