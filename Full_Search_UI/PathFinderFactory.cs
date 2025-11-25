using System;

namespace FullSearch
{
    public enum Algorithm
    {
        BreadthFirst,
        DepthFirst,
        HillClimbing,
        BestFirst,
        Dijkstra,
        AStar
    }

    public static class PathFinderFactory
    {
        public static IPathFinder NewPathFinder(Algorithm a)
        {
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
