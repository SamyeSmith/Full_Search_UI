using System;

namespace FullSearch
{
    // Enum representing different pathfinding algorithms - theese are used to select the appropriate algorithm in the factory method and used in the UI to let the user choose an algorithm.
    public enum Algorithm
    {
        BreadthFirst,
        DepthFirst,
        HillClimbing,
        BestFirst,
        Dijkstra,
        AStar
    }

    // Factory class to create instances of pathfinding algorithms based on the selected enum value.
    public static class PathFinderFactory
    {
        public static IPathFinder NewPathFinder(Algorithm a)
        {
            // Use a switch expression to return the appropriate pathfinding algorithm instance.
            // If the algorithm is not recognized, throw an exception.
            // Create and return the appropriate pathfinding algorithm instance based on the selected enum value.
            return a switch
            {
                Algorithm.BreadthFirst => new BreadthFirst(), 
                Algorithm.DepthFirst => new DepthFirst(), 
                Algorithm.HillClimbing => new HillClimbing(),
                Algorithm.BestFirst => new BestFirst(),
                Algorithm.Dijkstra => new Dijkstra(),
                Algorithm.AStar => new AStar(),
                _ => throw new ArgumentException("Unknown algorithm"),
            };
        }
    }
}
