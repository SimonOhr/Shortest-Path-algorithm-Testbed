using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortest_Path_Algorithm
{
    class Edge
    {
        public Node Parent { get; private set; }
        public Node Child { get; private set; }

        public Edge(Node parent, Node child)
        {
            Parent = parent;
            Child = child;
           // Console.WriteLine("Added Edge, Parent X = " + Parent.X + " parent Y = " + Parent.Y + "Edge to Node " + child.Y +" " + child.X);
        }
    }
}
