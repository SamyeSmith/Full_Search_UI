using System.Collections.Generic;

namespace FullSearch
{
    // Interface for pathfinding algorithms
    public interface IPathFinder
    {
        string Name { get; } // Name of the search algorithm
        List<Coord> FindPath(int[,] terrain, Coord start, Coord goal); // Method to find path
    }
}
