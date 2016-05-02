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
        List<Node> _CLOSED;

        private Node _SELECTED_NODE;

        private PriorityQueue _PIORITY_QUEUE;
        #endregion


        #region Properties
        public Map MAP { get { return this._MAP; } set { this._MAP = value; } }
        public int GRAPH_WIDTH { get { return this._GRAPH_WIDTH; } }
        public int GRAPH_HEIGHT { get { return this._GRAPH_HEIGHT; } }
        public int BLOCK_SIZE { get { return this._BLOCK_SIZE; } }
        public Node SELECTED_NODE { get { return this._SELECTED_NODE; } }
        public PriorityQueue PRIORITY_QUEUE { get { return this._PIORITY_QUEUE; }set { this._PIORITY_QUEUE = value; } }
        public List<Node> CLOSED { get { return this._CLOSED; } set { this._CLOSED = value; } }

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
            _CLOSED = new List<Node>();
            
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
        /// This is where A* will be implemented
        /// </summary>
        /// <param name="GOAL">This is the vertex that we start at</param>
        public void ShortestPath(Node GOAL)
        {
            // Set this so taht we can traverse later
            Node startingPoint = _SELECTED_NODE;

            // While the peek of open is not the goal
            while (_PIORITY_QUEUE.Peek() != GOAL)
            {
                // Take the current node out of open
                // Current should be the thing on top of the priority queue
                _SELECTED_NODE = _PIORITY_QUEUE.DequeueTwo();
                _CLOSED.Add(_SELECTED_NODE);

                // Find all of the neighbors of the current vertex
                List<Node> currentNeighbors = GetNieghbors(_SELECTED_NODE);

                foreach (Node neighbor in currentNeighbors)
                {
                    // Get the cost, which is G(current)
                    int cost = _SELECTED_NODE.DISTANCE + GetCost(_SELECTED_NODE, neighbor);

                    // If neighbor is in OPEN and cost less then neighbor.distance
                    // Neighbor.Distance is G(neighbor)
                    if (_PIORITY_QUEUE.heap.Contains(neighbor) && cost < neighbor.DISTANCE)
                    {
                        // Remove neighbor from OPEN, because the new path is better
                        _PIORITY_QUEUE.heap.Remove(neighbor);
                        _SELECTED_NODE = _PIORITY_QUEUE.DequeueTwo();
                    }
                    // If neighbor is in CLOSED and cost is less then g(neighbor)
                    if (_CLOSED.Contains(neighbor) && cost < neighbor.DISTANCE)
                    {
                        // Remove neighbor from CLOSED
                        _CLOSED.Remove(neighbor);
                    }
                    // If neighbor is not in open and neighbor is not in CLOSED:
                    if (!_PIORITY_QUEUE.heap.Contains(neighbor) && !_CLOSED.Contains(neighbor))
                    {
                        neighbor.DISTANCE = cost;
                        _PIORITY_QUEUE.Enqueue(neighbor);

                        neighbor.NEAREST_NODE = _SELECTED_NODE;
                    }
                }
            }

            // Reconstruct the reverse path from goal to start
            ColorPath(startingPoint, GOAL);
        }

        /// <summary>
        /// This method will be called if you want to do this as the 
        /// game plays, not all at once
        /// </summary>
        /// <param name="GOAL"></param>
        public void ShortestPathSlow(Node GOAL)
        {
            // Set this so taht we can traverse later
            Node startingPoint = _SELECTED_NODE;


            // Take the current node out of open
            // Current should be the thing on top of the priority queue
            _SELECTED_NODE = _PIORITY_QUEUE.DequeueTwo();
            _CLOSED.Add(_SELECTED_NODE);

            // Find all of the neighbors of the current vertex
            List<Node> currentNeighbors = GetNieghbors(_SELECTED_NODE);

            foreach (Node neighbor in currentNeighbors)
            {
                // Get the cost, which is G(current)
                int cost = _SELECTED_NODE.DISTANCE + GetCost(_SELECTED_NODE, neighbor);

                // If neighbor is in OPEN and cost less then neighbor.distance
                // Neighbor.Distance is G(neighbor)
                if (_PIORITY_QUEUE.heap.Contains(neighbor) && cost < neighbor.DISTANCE)
                {
                    // Remove neighbor from OPEN, because the new path is better
                    _PIORITY_QUEUE.heap.Remove(neighbor);
                    _SELECTED_NODE = _PIORITY_QUEUE.DequeueTwo();
                }
                // If neighbor is in CLOSED and cost is less then g(neighbor)
                if (_CLOSED.Contains(neighbor) && cost < neighbor.DISTANCE)
                {
                    // Remove neighbor from CLOSED
                    _CLOSED.Remove(neighbor);
                }
                // If neighbor is not in open and neighbor is not in CLOSED:
                if (!_PIORITY_QUEUE.heap.Contains(neighbor) && !_CLOSED.Contains(neighbor))
                {
                    neighbor.DISTANCE = cost;
                    _PIORITY_QUEUE.Enqueue(neighbor);

                    neighbor.NEAREST_NODE = _SELECTED_NODE;
                }
            }
        }

        /// <summary>
        /// This method checks if any vertex to the left, right, top or bottom
        /// of the given vertex, v, intersect with it. If they do, then add them to the list 
        /// and return that list when it has traversed all vertecies in the graph
        /// </summary>
        /// <param name="v">The vertex of which I am getting a list of all the neighbors</param>
        /// <returns>A list of vertecies that intersect with the given vertex(v)</returns>
        public List<Node> GetNieghbors(Node v)
        {
            List<Node> neighboringVertecies = new List<Node>();
            // Use collision detection to see which rectangles are neighbors
            // Make 4 different rectangles, checking one in each direction
            Rectangle leftCheck = v.RECTANGLE;
            leftCheck.X -= 3;
            Rectangle rightCheck = v.RECTANGLE;
            rightCheck.X += 3;
            Rectangle topCheck = v.RECTANGLE;
            topCheck.Y -= 3;
            Rectangle botCheck = v.RECTANGLE;
            botCheck.Y += 3;

            foreach (Node vertex in _ALL_NODES)
            {
                if (vertex != v)
                {
                    if (leftCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (rightCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (topCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                    if (botCheck.Intersects(vertex.RECTANGLE))
                    {
                        neighboringVertecies.Add(vertex);
                    }
                }

            }

            return neighboringVertecies;
        }

        /// <summary>
        /// This method will go backwards through the list and
        /// change each color of the tile to something along the 
        /// path
        /// </summary>
        public void ColorPath(Node start, Node goal)
        {
            Node currentPathVertex = goal;

            while (currentPathVertex != start)
            {
                if (currentPathVertex.NEAREST_NODE != null)
                {
                    currentPathVertex = currentPathVertex.NEAREST_NODE;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// This is a helper method that I will use to compute the cost
        /// The cost is the absoulte value of the (cur X - next X) + (cur Y - next Y)
        /// </summary>
        /// <returns>Absolute value of the difference between the distances</returns>
        public int GetCost(Node current, Node next)
        {
            return Math.Abs(current.RECTANGLE.X - next.RECTANGLE.X)
                + Math.Abs(current.RECTANGLE.Y - next.RECTANGLE.Y);
        }
        #endregion
    }
}
