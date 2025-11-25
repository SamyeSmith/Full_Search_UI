using System.Collections.Generic;
using System.Linq;

namespace FullSearch
{
    public class HillClimbing : IPathFinder
    {
        public string Name => "HillClimbing"; // the name of the algorithm

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal) // the main method to find a path
        {
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // get terrain dimensions
            var current = new SearchNode(start); // initialize current node
            var visited = new HashSet<Coord> { start }; // track visited positions

            while (true) // main loop
            {
                if (current.Position.Equals(goal)) return SearchUtilities.BuildPathList(current); // goal reached, build path

                var neighbors = SearchUtilities.GetNeighbors(current.Position) // get neighbors
                    .Where(nb => nb.Row >= 0 && nb.Row < rows && nb.Col >= 0 && nb.Col < cols) // within bounds
                    .Where(nb => terrain[nb.Row, nb.Col] != 0 && !visited.Contains(nb)) // passable and not visited
                    .Select(nb => new SearchNode(nb) { HCost = SearchUtilities.Manhattan(nb, goal), Predecessor = current }) // create nodes
                    .OrderBy(n => n.HCost) // sort by heuristic cost
                    .ToList(); // convert to list

                if (neighbors.Count == 0) return new List<Coord>(); // no more neighbors, return empty path

                current = neighbors.First(); // move to the best neighbor
                visited.Add(current.Position); // mark as visited
            }
        }
    }
}
