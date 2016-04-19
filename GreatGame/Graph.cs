using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class Graph
    {
        private Map map;
        private int width, height;  // Oveall width and height over the graph
        private int blockWidth, blockHeight;    // This is how big each one of the rectangles will be inside of the graph
        private List<Rectangle> graph;

        public Graph(Map m, int width, int height, int blockWidth, int blockHeight)
        {
            map = m;
            this.width = width;
            this.height = height;
            this.blockWidth = blockWidth;
            this.blockHeight = blockHeight;
            graph = new List<Rectangle>();
        }

        /// <summary>
        /// This method will use the coordinates of the map, and cross-check them with 
        /// the "graph" that I am gonna make. This "graph" will be a 
        /// </summary>
        public void Initialize()
        {
            // Use the width and height things to determine how many rectangles I need to make
            int maxWidth = 0, maxHeight = 0;

            foreach(Wall w in map.Walls)
            {
                // If the wall x position is greater then the current maxWidth, then chagne it
                // Same thing for the height
                if (w.Bounds.Max.X > maxWidth)
                {
                    maxWidth = (int)w.Bounds.Max.X;
                }
                if (w.Bounds.Max.Y > maxHeight)
                {
                    maxHeight = (int)w.Bounds.Max.Y;
                }
            }

            int numRowsTomake = maxHeight / blockHeight;
            int numColumnsToMake = maxWidth / blockWidth;



        }

    }
}
