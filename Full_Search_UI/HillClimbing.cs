using System.Collections.Generic;
using System.Linq;

namespace FullSearch
{
    public class HillClimbing : IPathFinder
    {
        public string Name => "HillClimbing";

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1);
            var current = new SearchNode(start);
            var visited = new HashSet<Coord> { start };

            while (true)
            {
                if (current.Position.Equals(goal)) return SearchUtilities.BuildPathList(current);

                var neighbors = SearchUtilities.GetNeighbors(current.Position)
                    .Where(nb => nb.Row >= 0 && nb.Row < rows && nb.Col >= 0 && nb.Col < cols)
                    .Where(nb => terrain[nb.Row, nb.Col] != 0 && !visited.Contains(nb))
                    .Select(nb => new SearchNode(nb) { HCost = SearchUtilities.Manhattan(nb, goal), Predecessor = current })
                    .OrderBy(n => n.HCost)
                    .ToList();

                if (neighbors.Count == 0) return new List<Coord>(); // stuck, fail

                current = neighbors.First();
                visited.Add(current.Position);
            }
        }
    }
}
