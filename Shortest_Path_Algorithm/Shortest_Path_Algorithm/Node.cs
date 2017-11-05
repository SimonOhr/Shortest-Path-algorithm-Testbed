﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortest_Path_Algorithm
{
    class Node
    {
      //  public int id;
        public int distance = int.MaxValue;
        public int weight = 1;
        public Node previous;
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<Edge> adj = new List<Edge>();
        private bool isStartNode;
        public bool hasBeenVisited;
        public bool setToBeVisited;
        public bool isTarget;
        public bool isPath;
        public Node(int x, int y)
        {
            X = x;
            Y = y;           
          //  Console.WriteLine("Added Node X = " + X + "Node Y = " + Y);

        }

        public void SetStartNode(bool setStartNode)
        {
            isStartNode = setStartNode;
        }
        public void SetVisited()
        {
            hasBeenVisited = true;
            setToBeVisited = false;
        }
        public void SetToVisit()
        {
            setToBeVisited = true;
        }
        public void IsPath()
        {
            isPath = true;
        }
    }
}
