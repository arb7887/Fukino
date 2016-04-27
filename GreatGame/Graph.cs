﻿using System;
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

        Dictionary<String, int> nodeNameToIndex;
        List<Node> nodes;

        Node selectedNode;

        /// <summary>
        /// This class will be used to do some A* pathfinding.
        /// Hopefully this will work, and I will make it based on the 
        /// A* homework.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="blockWidth"></param>
        /// <param name="blockHeight"></param>
        public Graph(Map m, int width, int height, int blockWidth, int blockHeight)
        {
            map = m;
            this.width = width;
            this.height = height;
            this.blockWidth = blockWidth;
            this.blockHeight = blockHeight;
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

            // Now we have the number of rows and coluns that we need to make.
            int x = 0 , y = 0;
            for(int i = 0; i < numRowsTomake; i++)
            {
                for(int j = 0; j < numColumnsToMake; j++)
                {
                    Rectangle rectangle = new Rectangle(x, y, blockWidth, blockHeight);
                    Node newNode = new Node(rectangle, false);
                    y += blockHeight;
                }
                x += blockWidth;
            }

        }


        /// <summary>
        /// This method will take a start vertex, and a final vertex.
        /// Also a ending vertex and it will find the shortest path
        /// between them by using A*
        /// </summary>
        public void ShortestPath(Node start)
        {
            // If this start is not a wall
        }
    }
}
