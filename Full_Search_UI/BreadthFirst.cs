using System.Collections.Generic;

namespace FullSearch
{
    public class BreadthFirst : IPathFinder
    {
        public string Name => "BreadthFirst";

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1);
            var visited = new bool[rows, cols];
            var q = new Queue<SearchNode>();
            var startNode = new SearchNode(start);
            q.Enqueue(startNode);
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
