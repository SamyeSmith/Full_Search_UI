using System;
using System.Collections.Generic;

namespace FullSearch
{
    // Utility class for search algorithms - general data and methods used across different search algorithms.
    public static class SearchUtilities
    {
        public static IEnumerable<Coord> GetNeighbors(Coord c)
        {
            yield return new Coord(c.Row - 1, c.Col); // North
            yield return new Coord(c.Row, c.Col + 1); // East
            yield return new Coord(c.Row + 1, c.Col); // South
            yield return new Coord(c.Row, c.Col - 1); // West
        }

        public static int Manhattan(Coord a, Coord b) // Heuristic function for A* search
        {
            return Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col); // Manhattan distance
        }

        public static List<Coord> BuildPathList(SearchNode endNode) // Reconstruct the path from the end node to the start node.
        {
            var path = new List<Coord>(); // List to hold the path coordinates
            var node = endNode; // Start from the end node
            while (node != null) // Traverse back to the start node
            {
                path.Add(node.Position); // Add the current node's position to the path
                node = node.Predecessor; // Move to the predecessor node
            }
            path.Reverse(); // Reverse the path to get it from start to end
            return path; // Return the constructed path
        }
    }
}
