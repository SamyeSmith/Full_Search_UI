using System;
using System.Collections.Generic;

namespace FullSearch
{
    public static class SearchUtilities
    {
        public static IEnumerable<Coord> GetNeighbors(Coord c)
        {
            yield return new Coord(c.Row - 1, c.Col); // North
            yield return new Coord(c.Row, c.Col + 1); // East
            yield return new Coord(c.Row + 1, c.Col); // South
            yield return new Coord(c.Row, c.Col - 1); // West
        }

        public static int Manhattan(Coord a, Coord b)
        {
            return Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);
        }

        public static List<Coord> BuildPathList(SearchNode endNode)
        {
            var path = new List<Coord>();
            var node = endNode;
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Predecessor;
            }
            path.Reverse();
            return path;
        }
    }
}
