﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreatGame
{
    class PriorityQueue
    {

        public List<Vertex> heap;

        public int Count { get { return heap.Count; } }

        public PriorityQueue()
        {
            heap = new List<Vertex>();
        }

        #region Methods
        /// <summary>
        /// Adds the given data to the priority queue
        /// </summary>
        /// <param name="data"></param>
        public void Enqueue(Vertex v)
        {
            // Add the data to the end of the list
            heap.Add(v);

            // Determine the index 
            int dataIndex = heap.Count - 1;

            // Use that index to find its parent index((index - 1) / 2)
            int parentIndex = ((dataIndex - 1) / 2);

            // Is the parent's value larger? if so, swap the parent and the
            // child Data
            if (heap[parentIndex].Distance > heap[dataIndex].Distance)
            {
                Vertex tempParent = heap[parentIndex];
                Vertex tempData = heap[dataIndex];

                heap[dataIndex] = tempParent;
                heap[parentIndex] = tempData;

                // Repeat this as many times as necessary
                Enqueue(heap[parentIndex]);
            }


        }

        /// <summary>
        /// Removes the highest priiority(smallest number) in
        /// the queue/heap
        /// </summary>
        /// <returns></returns>
        public Vertex Dequeue()
        {
            // This will be the root node since it's the smallest number
            // The root node is going to be the smallest number
            Vertex peek = Peek();

            int rootNodeIndex = 0;

            // Remove the last element of the List and put its data 
            // In the root's postion
            double lastNodeData = 0;

            if (heap.Count > 0)
            {
                lastNodeData = heap[heap.Count - 1].Distance;
                heap.Remove(heap[heap.Count - 1]);
            }


            // Put the last node's data in the root node's position
            heap[rootNodeIndex].Distance = lastNodeData;

            // Calculate the children's indexes
            int leftChildIndex = (2 * rootNodeIndex) + 1;
            int rightChildIndex = (2 * rootNodeIndex) + 2;

            // If the current value is greater then both children
            // Then swap with the smaller child
            if (heap[rootNodeIndex].Distance > heap[rightChildIndex].Distance
                && heap[rootNodeIndex].Distance > heap[leftChildIndex].Distance)
            {
                // Swap with the smaller child
                if (heap[leftChildIndex].Distance < heap[rightChildIndex].Distance)
                {
                    // Swap with the left
                    Swap(rootNodeIndex, leftChildIndex);
                }
                else
                {
                    // Swap with the right
                    Swap(rootNodeIndex, rightChildIndex);
                }
            }
            else if (heap[rootNodeIndex].Distance > heap[leftChildIndex].Distance)
            {
                // Swap with the right child
                Swap(rootNodeIndex, leftChildIndex);
            }
            else if (heap[rootNodeIndex].Distance > heap[rightChildIndex].Distance)
            {
                Swap(rootNodeIndex, rightChildIndex);
            }


            return peek;


        }


        // I was having problems with my Dequeue method, so I got this one from:
        // http://www.redblobgames.com/pathfinding/a-star/implementation.html#sec-4-3
        /// <summary>
        /// Removes the head of the queue (node with minimum priority; ties are broken by order of insertion), and returns it.
        /// If queue is empty, result is undefined
        /// O(log n)
        /// </summary>
        public Vertex DequeueTwo()
        {
            int bestIndex = 0;

            for (int i = 0; i < heap.Count; i++)
            {
                if (heap[i].Distance < heap[bestIndex].Distance)
                {
                    bestIndex = i;
                }
            }

            Vertex bestItem = heap[bestIndex];
            heap.RemoveAt(bestIndex);
            return bestItem;
        }

        /// <summary>
        /// This is a helper method that will just swap these two indexes in 
        /// the heap
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        public void Swap(int firstIndex, int secondIndex)
        {
            Vertex tempFirstData = heap[firstIndex];
            Vertex tempSecData = heap[secondIndex];

            heap[firstIndex] = tempSecData;
            heap[secondIndex] = tempFirstData;
        }

        /// <summary>
        /// Return the first piece of data in the lis
        /// This should be the smallest thing in the list
        /// </summary>
        /// <returns></returns>
        public Vertex Peek()
        {
            if (!IsEmpty())
                return heap[0];
            else
                return null;

        }

        /// <summary>
        /// This determines if the list is empty or not
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            if (heap.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calls the Clear method on the List object
        /// </summary>
        public void Clear()
        {
            heap.Clear();
        }
        #endregion
    }

}
