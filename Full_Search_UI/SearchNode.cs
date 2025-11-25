using System;
using System.Collections.Generic;

namespace FullSearch
{
    // Coordinate structure representing a position in the grid.
    public struct Coord
    {
        public int Row { get; set; } // Row index
        public int Col { get; set; } // Column index

        public Coord(int r, int c) { Row = r; Col = c; } // Constructor to initialize row and column.

        public override string ToString() => $"{Row} {Col}"; // String representation of the coordinate.

        public override bool Equals(object obj) // Checking if the row and column are equal.
        {
            if (!(obj is Coord)) return false; // Check if the object is of type Coord.
            var o = (Coord)obj; // Cast the object to Coord.
            return Row == o.Row && Col == o.Col; // Compare row and column values.
        }

        public override int GetHashCode() => HashCode.Combine(Row, Col); // Generate hash code based on row and column.
    }

    
    public class SearchNode
    {
        public Coord Position { get; set; } // Position of the node in the grid.
        public SearchNode Predecessor { get; set; } // Predecessor node in the path.
        public int GCost { get; set; } // Cost from the start node to this node.
        public int HCost { get; set; } // Huristic cost from this node to the goal node.
        public int FCost => GCost + HCost; // Total cost (Total Cost of huristics and cost from start to current node).

        public SearchNode(Coord pos) // Constructor to initialize the node with a position.
        {
            Position = pos; // Set the position.
            Predecessor = null; // No predecessor initially.
            GCost = 0; // Initial cost from start is 0.
            HCost = 0; // Initial heuristic cost is 0.
        }
    }
}
