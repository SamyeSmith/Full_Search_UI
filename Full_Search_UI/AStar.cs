using System.Collections.Generic;

namespace FullSearch
{
    public class AStar : IPathFinder // Implements the A* pathfinding algorithm
    {
        public string Name => "AStar"; // Name of the algorithm
        public int OpenListSorts = 0;

        // Main method to find the path from start to goal
        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // Get dimensions of the terrain
            var closed = new bool[rows, cols]; // Closed list to track visited nodes
            var open = new PriorityQueue(); // Open list as a priority queue
            var startNode = new SearchNode(start) { GCost = 0, HCost = SearchUtilities.Manhattan(start, goal) }; // Initialize start node
            open.Enqueue(startNode); // Add start node to open list

            while (!open.IsEmpty()) // Main loop, the while also acts as error handling for no path found
            {
                var node = open.Dequeue(); // Get the node with the lowest F cost
                if (closed[node.Position.Row, node.Position.Col]) continue; // Skip if already in closed list
                closed[node.Position.Row, node.Position.Col] = true; // Mark node as visited

                if (node.Position.Equals(goal)) // Check if goal is reached
                {
                    return SearchUtilities.BuildPathList(node); // Build and return the path
                }

                foreach (var nb in SearchUtilities.GetNeighbors(node.Position)) // Explore neighbors
                {
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue; // Skip out-of-bounds neighbors
                    if (terrain[nb.Row, nb.Col] == 0) continue; // Skip impassable terrain
                    if (closed[nb.Row, nb.Col]) continue; // Skip already visited neighbors

                    int tentativeG = node.GCost + terrain[nb.Row, nb.Col]; // Calculate tentative G cost
                    var newNode = new SearchNode(nb) { Predecessor = node, GCost = tentativeG, HCost = SearchUtilities.Manhattan(nb, goal) }; // Create new search node
                    open.Enqueue(newNode); // Add neighbor to open list
                }
            }

            return new List<Coord>(); // Return empty path if no path is found
        }
    }
}
