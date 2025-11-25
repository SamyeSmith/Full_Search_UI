using System.Collections.Generic;

namespace FullSearch
{
    public class AStar : IPathFinder
    {
        public string Name => "AStar";
        public int OpenListSorts = 0;

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1);
            var closed = new bool[rows, cols];
            var open = new PriorityQueue();
            var startNode = new SearchNode(start) { GCost = 0, HCost = SearchUtilities.Manhattan(start, goal) };
            open.Enqueue(startNode);

            while (!open.IsEmpty())
            {
                var node = open.Dequeue();
                if (closed[node.Position.Row, node.Position.Col]) continue;
                closed[node.Position.Row, node.Position.Col] = true;

                if (node.Position.Equals(goal)) 
                {
                    return SearchUtilities.BuildPathList(node);
                }

                foreach (var nb in SearchUtilities.GetNeighbors(node.Position))
                {
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols) continue;
                    if (terrain[nb.Row, nb.Col] == 0) continue;
                    if (closed[nb.Row, nb.Col]) continue;

                    int tentativeG = node.GCost + terrain[nb.Row, nb.Col];
                    var newNode = new SearchNode(nb) { Predecessor = node, GCost = tentativeG, HCost = SearchUtilities.Manhattan(nb, goal) };
                    open.Enqueue(newNode);
                }
            }

            return new List<Coord>();
        }
    }
}
