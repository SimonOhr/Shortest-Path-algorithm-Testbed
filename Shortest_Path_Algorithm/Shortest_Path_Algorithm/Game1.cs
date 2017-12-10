using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Shortest_Path_Algorithm
{
    /// <summary>
    /// this was just a testbed for a quoridor project, in which we had limited access to the source code. As such, come conventions were overlooked
    /// </summary>
    public class Game1 : Game
    {
        enum GameState { pathing, initializing }
        GameState currentGameState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D rectTex;
        Tile[,] tiles;
        //Graph
        List<Edge> edges = new List<Edge>();
        Node[,] nodes;
        List<Node> nodeList = new List<Node>();
        //Path-Finding
        Queue<Node> visit = new Queue<Node>();
        Queue<Node> visited = new Queue<Node>();
        Queue<Node> path = new Queue<Node>();
        int edgeDistance = 0;
        int newDistance = 0;
        Node[] thisIsMyPath;
        //DEBUGG
        bool timerOn = true; // DEBUGG used for timer to slow down pathing
        int targetY = 8; // DEBUGG target
        int targetX = 1; // DEBUGG target
        double counter = 0; // DEBUGG used for timer to slow down pathing
        bool setDebuggOn;
        MouseState mousePos, oldMousePos;
        KeyboardState keyState, oldKeystate;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            rectTex = Content.Load<Texture2D>("vitRektangel");

            currentGameState = GameState.initializing;

            tiles = new Tile[9, 9];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    int y = i * 60;
                    int x = j * 60;
                    tiles[i, j] = new Tile(rectTex, new Vector2(x, y));
                }
            }
            nodes = new Node[9, 9];

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    int x = j;
                    int y = i;
                    nodes[i, j] = new Node(x, y);
                    nodeList.Add(nodes[i, j]);
                }
            }

            IsMouseVisible = true;

            //rectangles = new Rectangle[9, 9];
            //for (int i = 0; i < rectangles.GetLength(0); i++)
            //{
            //    for (int j = 0; j < rectangles.GetLength(1); j++)
            //    {
            //        int y = i * 60;                    
            //        int x = j * 60;
            //        rectangles[i, j] = new Rectangle(rectTex, new Vector2(x, y));
            //    }
            //}
            //nodes = new Node[9, 9];

            //for (int i = 0; i < rectangles.GetLength(0); i++)
            //{
            //    for (int j = 0; j < rectangles.GetLength(1); j++)
            //    {
            //        int x = j;
            //        int y = i;
            //        nodes[i, j] = new Node(x, y);
            //        nodeList.Add(nodes[i, j]);
            //    }
            //}
            //AddEdges(ref edges, ref nodes);
            //SetAdj(ref edges, ref nodeList);
            ////DoDebugg(); // UPS, takes a looong time
            ////PrintGraph();
            //thisIsMyPath = DoDijkstraAlgo(nodes[0, 4]);

            //if (thisIsMyPath != null)
            //{                
            //    foreach (Node n in thisIsMyPath)
            //    {
            //        if (n != null) n.IsPath();
            //    }
            //}
        }

        private void MakeGraphDoAlgo()
        {

            AddEdges(ref edges, ref nodes);
            SetAdj(ref edges, ref nodeList);
            //DoDebugg(); // UPS, takes a looong time
            //PrintGraph();
            thisIsMyPath = DoDijkstraAlgo(nodes[0, 4]);

            if (thisIsMyPath != null)
            {
                foreach (Node n in thisIsMyPath)
                {
                    if (n != null) n.IsPath();
                }
            }
        }
        /// <summary>
        /// Debugg function, beware, it will print out alot
        /// </summary>
        private void DoDebugg()
        {
            Console.WriteLine("Nodes: " + nodeList.Count);
            Console.WriteLine("Edges: " + edges.Count);
            setDebuggOn = true;
            int counter = 0;
            int sum = 0;
            foreach (Node n in nodeList)
            {
                Console.Write(n.adj.Count);
                counter++;
                sum += n.adj.Count;
                if (counter >= 9)
                {
                    Console.WriteLine("\n");
                    counter = 0;
                }
            }
        }
        /// <summary>
        /// slimmer version of the debugg function above, only prints the graph in a visual representation to the output
        /// </summary>
        private void PrintGraph()
        {
            int counter = 0;
            int sum = 0;
            foreach (Node n in nodeList)
            {
                Console.Write(n.adj.Count);
                counter++;
                sum += n.adj.Count;
                if (counter >= 9)
                {
                    Console.WriteLine("\n");
                    counter = 0;
                }
            }
        }
        /// <summary>
        /// Creates edge-classes containing the origin node and a neighbor.
        /// this function can be refactored so that each node gets inserted into the corresponding nodes list of adjecencies,
        /// eliminating the next, rather time consuming function
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="nodes"></param>
        private void AddEdges(ref List<Edge> edges, ref Node[,] nodes)
        {
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (Decrement(j) != -1)
                    {
                        edges.Add(new Edge(nodes[i, j], nodes[i, j - 1]));
                    }
                    if (Increment(j) != -1)
                    {
                        edges.Add(new Edge(nodes[i, j], nodes[i, j + 1]));
                    }

                    if (Decrement(i) != -1)
                    {
                        edges.Add(new Edge(nodes[i, j], nodes[i - 1, j]));
                    }

                    if (Increment(i) != -1)
                    {
                        edges.Add(new Edge(nodes[i, j], nodes[i + 1, j]));
                    }
                }
            }
        }
        /// <summary>
        /// Decrement / Increment functions, for checking the bounds of the array.
        /// Made the function above look atleast a bit cleaner. But in the unlikely event, I find the time of revisiting this testbed (maybe for future projects)
        /// these functions will most likely be incorperated into the above function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int Decrement(int x)
        {
            int value = x;
            value--;
            if (value < 0)
            {
                return -1;
            }
            else
            {
                return value;
            }
        }
        private int Increment(int x)
        {
            int value = x;
            value++;
            if (value > 8)
            {
                return -1;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// applies vertices to the corresponding list of adjecencies for each node, this is can be done faster as explained above. At the very least,
        /// it wont have to do both operations separately, neither would it have to traverse and check 288 edges and input these into 89 nodes.
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="nodeList"></param>
        private void SetAdj(ref List<Edge> edges, ref List<Node> nodeList)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                for (int j = 0; j < nodeList.Count; j++)
                {
                    if (edges[i].Parent.X == nodeList[j].X && edges[i].Parent.Y == nodeList[j].Y) // O~N^2
                    {
                        nodeList[j].adj.Add(edges[i]); // edges.RemoveAt(i) ? 
                        if (setDebuggOn) Console.WriteLine("Added Edge with Parent X = " + edges[i].Parent.X + " Parent Y = " + edges[i].Parent.Y + "To Node At X = " + edges[i].Child.X + " And Y = " + edges[i].Child.Y);
                        //  edges.RemoveAt(i);
                    }
                }
            }
        }
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.pathing:
                    MakeGraphDoAlgo();
                    currentGameState = GameState.initializing;
                    break;
                case GameState.initializing:
                    oldMousePos = mousePos;
                    mousePos = Mouse.GetState();
                    oldKeystate = keyState;
                    keyState = Keyboard.GetState();
                    for (int i = 0; i < tiles.GetLength(0); i++)
                    {
                        for (int j = 0; j < tiles.GetLength(1); j++)
                        {
                            if (tiles[i, j].boundingBox.Contains(mousePos.Position) && mousePos.LeftButton == ButtonState.Pressed)
                            {
                                tiles[i, j].SetObstacle();
                                nodes[i, j].weight = 100;
                            }
                        }
                    }

                    if (keyState.IsKeyDown(Keys.Enter) && oldKeystate.IsKeyUp(Keys.Enter)) currentGameState = GameState.pathing;                 
                    break;
                default:
                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // most of the booleans are exclusive for the purposes of the testbed, color coding how the algorithm functions, paths, and goals etc.
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (nodes[i, j].hasBeenVisited)
                    {
                        tiles[i, j].Update();
                        tiles[i, j].SetActive();
                    }
                    if (nodes[i, j].setToBeVisited)
                    {
                        tiles[i, j].Update();
                        tiles[i, j].SetInProgress();
                    }
                    if (nodes[i, j].Y == targetY && nodes[i, j].X == targetX)
                    {
                        tiles[i, j].Update();
                        tiles[i, j].SetTarget();
                    }
                    if (nodes[i, j].isPath)
                    {
                        tiles[i, j].Update();
                        tiles[i, j].SetPath();
                    }
                }
            }

            //if (counter <= 1 && timerOn)
            //{
            //    counter += gt.ElapsedGameTime.TotalSeconds;
            //}
            //else
            //{                
            //    if (thisIsMyPath != null)
            //    {
            //        timerOn = false;
            //        foreach (Node n in thisIsMyPath)
            //        {
            //            if (n != null) n.IsPath();
            //        }
            //    }
            //    else
            //    {
            //        thisIsMyPath = DoDijkstraAlgo(nodes[4,4]);

            //        counter = 0;
            //    }               
            //}                              
            base.Update(gameTime);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);

        }
        /// <summary>
        /// Can also be refactored, in the actual project, Dijkstras algorithm had to also take into account wall placements,
        /// it had to be done in a different manner from teh testbed, since the walls in the project weren't applied to the acutal tiles.
        /// Movement onto a tile affected by a nearby wall was tricky. As a wall affected 4 tiles, and the algorithm had to understand that moving alongside the walls was ok.  
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private Node[] DoDijkstraAlgo(Node start)
        {
            start.distance = 0;
            start.weight = 0;
            start.SetStartNode(true);
            start.SetToVisit();
            visit.Enqueue(start);

            while (visit.Count != 0)
            {
                Node u = visit.Dequeue();
                if (setDebuggOn) Console.WriteLine("Evaluated NodeYX: " + u.Y + " " + u.X);
                EvaluateNeighbors(ref u, ref visit);

                if (u.Y == targetY /*&& u.X == targetX*/)
                {
                    Node[] pathArray = new Node[81];
                    Node target = u;

                    if (setDebuggOn)
                    {
                        for (int i = 0; i < visited.Count; i++)
                        {
                            Console.WriteLine("path detected, nodes in queue: " + visited.Dequeue().Y + visited.Dequeue().X);
                            Console.WriteLine("Target found: " + target + " at index " + target.Y + " " + target.X);
                        }
                    }

                    path.Enqueue(target);
                    while (target.previous != null)
                    {
                        target = target.previous;
                        path.Enqueue(target);
                    }
                    path.Enqueue(start);

                    for (int i = 0; i < path.Count; i++)
                    {
                        Node[] tempArray = path.ToArray();
                        if (setDebuggOn) Console.WriteLine("path; " + tempArray[i].Y + " " + tempArray[i].X);
                    }
                    for (int i = path.Count - 1; i >= 0; --i)
                    {
                        pathArray[i] = path.Dequeue();
                    }
                    int it = 0;

                    while (pathArray[it] != null)
                    {
                        if (setDebuggOn) Console.WriteLine("Path after Reversal, Index " + it + " contains Node at index " + pathArray[it].Y + " " + pathArray[it].X);
                        it++;
                    }
                    return pathArray;
                }

                u.SetVisited();
                visited.Enqueue(u);
            }
            return null;

        }
        private void EvaluateNeighbors(ref Node u, ref Queue<Node> visit)
        {
            foreach (Edge v in u.adj)
            {
                Node adj = v.Child;
                if (setDebuggOn) Console.WriteLine("Found NodeYX " + adj.Y + " " + adj.X + " Adj to " + u.Y + " " + u.X);


                int edgeDistance = u.weight + adj.weight;

                // int newDistance = u.distance + edgeDistance;
                if (edgeDistance < adj.distance && adj.weight < 2)
                {
                    if (setDebuggOn) Console.WriteLine("AdjNode edgeDistance " + edgeDistance + " : newDistance " + newDistance + " adj.distance = " + adj.distance);
                    adj.distance = edgeDistance;
                    adj.previous = u;
                    adj.SetToVisit();
                    visit.Enqueue(adj);
                    if (setDebuggOn) Console.WriteLine("AdjNode edgeDistance " + edgeDistance + " : newDistance " + newDistance + " new adj.distance = " + adj.distance);
                }
            }
        }
    }

}
