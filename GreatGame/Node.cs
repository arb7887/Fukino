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
        #region Fields
        private Rectangle rectangle;
        public bool isObsticle;
        private Node nearestNode;
        private int distance;
        private bool permanent;
        #endregion


        #region Constructor
        public Node(Rectangle rect, bool isObsticle)
        {
            this.rectangle = rect;
            this.isObsticle = isObsticle;
            Reset();

        }

        public Node(Point location, Point size, bool isObsticle)
        {
            rectangle = new Rectangle(location, size);
            this.isObsticle = isObsticle;

            Reset();
        }
        #endregion


        #region Properties
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }
        #endregion


        #region Methods
        public void Reset()
        {
            permanent = false;
            distance = int.MaxValue;
            nearestNode = null;
            
        }
        #endregion
    }
}
