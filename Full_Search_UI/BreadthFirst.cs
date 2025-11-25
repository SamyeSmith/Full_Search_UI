using System.Collections.Generic;

namespace FullSearch
{
    public class BreadthFirst : IPathFinder
    {
        public string Name => "BreadthFirst"; // name for for display 

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal) // returns list of coordinates from start to goal
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // dimensions of terrain
            var visited = new bool[rows, cols]; // track visited positions
            var q = new Queue<SearchNode>(); // queue for BFS
            var startNode = new SearchNode(start); // create start node
            q.Enqueue(startNode); // enqueue start node
            visited[start.Row, start.Col] = true;

            while (!q.IsEmpty())
            {
                var node = q.Dequeue();
                if (node.Position.Equals(goal)) return SearchUtilities.BuildPathList(node);

                foreach (var nb in SearchUtilities.GetNeighbors(node.Position))
                {
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue;
                    if (terrain[nb.Row, nb.Col] == 0) continue; // wall
                    if (visited[nb.Row, nb.Col]) continue;

                    visited[nb.Row, nb.Col] = true;
                    var newNode = new SearchNode(nb) { Predecessor = node };
                    q.Enqueue(newNode);
                }
            }

            return new List<Coord>(); // no path
        }
    }
}
