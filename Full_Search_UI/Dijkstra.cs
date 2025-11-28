using System.Collections.Generic;

namespace FullSearch
{
    public class Dijkstra : IPathFinder
    {
        public string Name => "Dijkstra"; // name for for dropdown display

        // Main Dijkstra pathfinding method, passes through key data structures
        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // dimensions of terrain

            var closed = new bool[rows, cols]; // closed set to track visited nodes

            var open = new PriorityQueue(); // priority queue for open set

            var startNode = new SearchNode(start) { GCost = 0 }; // start node with 0 cost

            open.Enqueue(startNode); // add start node to open set

            while (!open.IsEmpty()) // main loop
            {
                var node = open.Dequeue(); // get node with lowest cost

                if (closed[node.Position.Row, node.Position.Col]) continue; // skip if already visited

                closed[node.Position.Row, node.Position.Col] = true; // mark node as visited

                if (node.Position.Equals(goal)) return SearchUtilities.BuildPathList(node); // goal reached, build and return path

                foreach (var nb in SearchUtilities.GetNeighbors(node.Position)) // explore neighbors
                {
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue; // bounds check

                    if (terrain[nb.Row, nb.Col] == 0) continue; // skip impassable terrain

                    if (closed[nb.Row, nb.Col]) continue; // skip if neighbor already visited

                    var cost = node.GCost + terrain[nb.Row, nb.Col]; // calculate cost to neighbor

                    var newNode = new SearchNode(nb) { Predecessor = node, GCost = cost }; // create new search node

                    open.Enqueue(newNode); // add neighbor to open set
                }
            }

            return new List<Coord>(); // no path found, return empty list
        }
    }
}
