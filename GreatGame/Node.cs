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
        public bool isObsticle;
        private Node nearestNode;
        private int distance;
        private bool permanent;

        public Node(Rectangle rect, bool isObsticle)
        {
            this.rect = rect;
            this.isObsticle = isObsticle;
            Reset();

        }

        public Rectangle Rect { get { return rect; } set { rect = value; } }

        public void Reset()
        {
            permanent = false;
            distance = int.MaxValue;
            nearestNode = null;
            
        }
    }
}
