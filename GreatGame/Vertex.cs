using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{

    class Vertex
    {
        #region Fields
        private Color vertColor;
        private Vertex neighborVertex;
        private Boolean permanent;
        private Rectangle rectangle;
        private int distance;
        #endregion


        #region Properties
        public Color VertColor { get { return vertColor; } set { this.vertColor = value; } }
        public Vertex NeighboringVertex { get { return neighborVertex; } set { this.neighborVertex = value; } }
        public Boolean Permanent { get { return permanent; } set { this.permanent = value; } }
        public Rectangle RECTANGLE { get { return this.rectangle; } set { this.rectangle = value; } }
        public int Distance { get { return this.distance; } set { this.distance = value; } }

        #endregion


        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"> This point is for the location of the rectangle</param>
        /// <param name="size"> This is for the size of the block </param>
        public Vertex(Point location, Point size)
        {
            // Make a new rectangle with the given information
            rectangle = new Rectangle(location, size);

            // Default this
            Reset();
        }
        #endregion


        #region Methods
        /// <summary>
        /// Resets the vertex's color to white, permanent to false
        /// and the distance to int.MaxValue. Neighbor vertex to null.
        /// </summary>
        public void Reset()
        {
            vertColor = Color.White;
            permanent = false;
            distance = int.MaxValue;
            neighborVertex = null;
        }

        /// <summary>
        /// Draws the rectangle of this object along with it's color using
        /// the texture that gets passed in
        /// </summary>
        /// <param name="sb">Spritebatch used to draw </param>
        /// <param name="pixel">The pixel texture that should be white in order to tint any color</param>
        public void Draw(SpriteBatch sb, Texture2D pixel, Color color)
        {

            sb.Draw(pixel, RECTANGLE, color);

        }
        #endregion
    }
}
