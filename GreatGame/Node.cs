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
        private Rectangle _RECTANGLE;
        public bool _IS_OBSTICLE;
        private Node _NEAREST_NODE;
        private int _DISTANCE;
        private bool _PERMANENT;
        #endregion


        #region Constructor
        public Node(Rectangle rect, bool isObsticle)
        {
            this._RECTANGLE = rect;
            this._IS_OBSTICLE = isObsticle;
            Reset();

        }

        public Node(Point location, Point size, bool isObsticle)
        {
            _RECTANGLE = new Rectangle(location, size);
            this._IS_OBSTICLE = isObsticle;

            Reset();
        }
        #endregion


        #region Properties
        public Rectangle RECTANGLE { get { return _RECTANGLE; } set { _RECTANGLE = value; } }
        public int DISTANCE { get { return this._DISTANCE; } set { this._DISTANCE = value; } }
        public bool IS_OBSTICLE { get { return this._IS_OBSTICLE; } set { this._IS_OBSTICLE = value; } }
        public bool PERMANENT { get { return this._PERMANENT; } set { this._PERMANENT = value; } }
        public Node NEAREST_NODE { get { return this._NEAREST_NODE; }  set { this._NEAREST_NODE = value; } }
        #endregion


        #region Methods
        public void Reset()
        {
            _PERMANENT = false;
            _DISTANCE = int.MaxValue;
            _NEAREST_NODE = null;
            
        }
        #endregion
    }
}
