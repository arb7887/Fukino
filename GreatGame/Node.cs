using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Node
    {
        private Rectangle rect;
        public enum terrainType { wall, objective, terrain }

        public Node(Rectangle rect)
        {
            this.rect = rect;

        }

        public Rectangle Rect { get { return rect; } set { rect = value; } }

    }
}
