using System.Collections.Generic;
using System.Linq;

namespace FullSearch
{
    public class HillClimbing : IPathFinder
    {
        public string Name => "HillClimbing";

        public List<Coord> FindPath(int[,] terrain, Coord start, Coord goal)
        {
            int rows = terrain.GetLength(0);
            int cols = terrain.GetLength(1);

            // ----- Lists used for this version -----
            var openList = new List<SearchNode>();     // nodes waiting to be checked
            var closedList = new HashSet<Coord>();     // nodes already checked
            var tempList = new List<SearchNode>();     // temporary neighbors

            // Start node
            var current = new SearchNode(start);
            openList.Add(current);

            while (openList.Count > 0)
            {
                // Pop the first element from the open list
                current = openList[0];
                openList.RemoveAt(0);
                closedList.Add(current.Position);

                // Goal check
                if (current.Position.Equals(goal))
                    return SearchUtilities.BuildPathList(current);

                // ----- Build temp list of neighbors -----
                tempList.Clear();

                foreach (var nb in SearchUtilities.GetNeighbors(current.Position))
                {
                    // in bounds?
                    if (nb.Row < 0 || nb.Row >= rows || nb.Col < 0 || nb.Col >= cols)
                        continue;

                    // blocked or already closed?
                    if (terrain[nb.Row, nb.Col] == 0 || closedList.Contains(nb))
                        continue;

                    // create a neighbor node
                    var node = new SearchNode(nb)
                    {
                        Predecessor = current,
                        HCost = SearchUtilities.Manhattan(nb, goal)
                    };

                    tempList.Add(node);
                }

                // if no neighbors → stuck
                if (tempList.Count == 0)
                    return new List<Coord>();

                // ----- Hill climbing chooses only the best neighbor -----
                var best = tempList.OrderBy(n => n.HCost).First();

                // Open list only gets 1 item → the best one
                openList.Clear();
                openList.Add(best);
            }

            return new List<Coord>(); // no path
        }
    }
}