using System.Collections.Generic;

namespace FullSearch
{
    public class DepthFirst : IPathFinder
    {
        public string Name => "DepthFirst"; // name for for display, will be shown in the combo box

        // Depth-First Search implementation
        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // dimensions of the terrain
            var visited = new bool[rows, cols];// track visited positions
            var stack = new Stack<SearchNode>(); // stack for DFS
            var startNode = new SearchNode(start); // starting node
            stack.Push(startNode); // push start node onto stack

            while (!stack.IsEmpty()) // main DFS loop, with while acting as error prevention
            {
                var node = stack.Pop(); // get the top node from the stack
                if (visited[node.Position.Row, node.Position.Col]) continue; // skip if already visited
                visited[node.Position.Row, node.Position.Col] = true; // mark as visited

                if (node.Position.Equals(goal)) return SearchUtilities.BuildPathList(node); // goal check

                var neighbors = new List<Coord>(SearchUtilities.GetNeighbors(node.Position)); // get neighbors
                for (int i = neighbors.Count - 1; i >= 0; i--) // inverse order for stack
                {
                    var nb = neighbors[i]; // neighbor coordinate
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue; // bounds check
                    if (terrain[nb.Row, nb.Col] == 0) continue; // impassable terrain check
                    if (visited[nb.Row, nb.Col]) continue; // already visited check
                    var newNode = new SearchNode(nb) { Predecessor = node }; // create new node
                    stack.Push(newNode); // push neighbor node onto stack
                }
            }

            return new List<Coord>(); // return empty path if no path found
        }
    }
}
