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
        #region Fields

        private Map _MAP;
        private int _GRAPH_WIDTH, _GRAPH_HEIGHT;  // Oveall width and height over the graph
        private int _BLOCK_SIZE;    // This is how big each one of the rectangles will be inside of the graph

        Dictionary<String, int> _NODE_NAME_TO_INDEX;
        List<Node> _ALL_NODES;

        private Node _SELECTED_NODE;
        #endregion


        #region Properties
        public Map MAP { get { return this._MAP; } set { this._MAP = value; } }
        public int GRAPH_WIDTH { get { return this._GRAPH_WIDTH; } }
        public int GRAPH_HEIGHT { get { return this._GRAPH_HEIGHT; } }
        public int BLOCK_SIZE { get { return this._BLOCK_SIZE; } }
        public Node SELECTED_NODE { get { return this._SELECTED_NODE; } }

        #endregion


        #region Constructor
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
        public Graph(Map m, int graphWidth, int graphHeight, int blockSize)
        {
            _MAP = m;

            this._GRAPH_WIDTH = graphWidth;
            this._GRAPH_HEIGHT = graphHeight;
            this._BLOCK_SIZE = blockSize;

            _NODE_NAME_TO_INDEX = new Dictionary<string, int>();
            _ALL_NODES = new List<Node>();
            
        }
        #endregion


        #region Methods
        /// <summary>
        /// This method will use the coordinates of the map, and cross-check them with 
        /// the "graph" that I am gonna make. This "graph" will be a 
        /// </summary>
        public void Initialize()
        {
            // Use the width and height things to determine how many rectangles I need to make
            int maxWidth = 0, maxHeight = 0;

            foreach(Wall w in _MAP.Walls)
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

            // Loop throgh and make the number of rows
            for (int x = 0; x < maxWidth; x += _BLOCK_SIZE)
            {
                // For each row, make a column
                for (int y = 0; y < maxHeight; y += _BLOCK_SIZE)
                {
                    // Make a new Vertex and add it to OPEN
                    // Use blocksize - 1 in order to draw a little bit of space in between the blocks
                    Node newNode = new Node(new Point(x, y), new Point(_BLOCK_SIZE), false);
                    _ALL_NODES.Add(newNode);
                }
            }

            // Select an initial node

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
        #endregion
    }
}
