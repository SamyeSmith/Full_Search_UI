using System.Collections.Generic;

namespace FullSearch
{
    public class BestFirst : IPathFinder
    {
        public string Name => "BestFirst"; // Name of the algorithm for display and enum

        // main metod for Best First Search
        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // get dimensions of the terrain
            var visited = new bool[rows, cols]; // track visited nodes
            var open = new PriorityQueue(); // priority queue for open nodes
            var startNode = new SearchNode(start) { HCost = SearchUtilities.Manhattan(start, goal) }; // create start node with manhattan cost
            open.Enqueue(startNode); // add start node to open list

            while (!open.IsEmpty()) // main loop, with while acting as error prevention
            {
                var node = open.Dequeue(); // get node with lowest manhattan cost
                if (visited[node.Position.Row, node.Position.Col]) continue; // skip if already visited
                visited[node.Position.Row, node.Position.Col] = true; // mark node as visited

                if (node.Position.Equals(goal)) return SearchUtilities.BuildPathList(node); // if goal is reached, build and return path

                foreach (var nb in SearchUtilities.GetNeighbors(node.Position)) // explore neighbors
                {
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue; // skip out-of-bounds
                    if (terrain[nb.Row, nb.Col] == 0) continue; // skip non-traversable terrain
                    if (visited[nb.Row, nb.Col]) continue; // skip already visited
                    var newNode = new SearchNode(nb) { Predecessor = node, HCost = SearchUtilities.Manhattan(nb, goal) }; // create new node for neighbor
                    open.Enqueue(newNode); // add neighbor to open list
                }
            }

            return new List<Coord>(); // return empty path if no path found
        }
    }
}
