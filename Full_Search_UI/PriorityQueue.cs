using System;
using System.Collections.Generic;

namespace FullSearch
{
    // Priority queue for SearchNode and A* algorithm.
    public class PriorityQueue
    {
        private List<SearchNode> heap = new List<SearchNode>(); 
        public int Count => heap.Count; // Number of elements in the priority queue.

        private bool Compare(SearchNode a, SearchNode b)
        {
            if (a.FCost != b.FCost) return a.FCost < b.FCost; // Compare by FCost first. (FCost is total cost)
            return a.HCost < b.HCost; // If FCost is equal, compare by HCost. (Hcost is heuristic cost)
        }

        public void Enqueue(SearchNode node) // Add a node to the priority queue.
        {
            heap.Add(node); // Add to the end of the heap.
            SiftUp(heap.Count - 1); // Restore heap property by sifting up.
        }

        public SearchNode Dequeue() // Remove and return the node with the highest priority (lowest FCost).
        {
            if (heap.Count == 0) throw new InvalidOperationException("PriorityQueue empty"); // Check for empty queue.
            var root = heap[0]; // The root of the heap (highest priority).
            var last = heap[heap.Count - 1]; // Get the last element.
            heap.RemoveAt(heap.Count - 1); // Remove the last element.
            if (heap.Count > 0) // If there are still elements left.
            {
                heap[0] = last; // Move the last element to the root.
                SiftDown(0); // Restore heap property by sifting down.
            }
            return root; // Return the removed root node.
        }

        public void SiftUp(int i) // Restore heap property by sifting up from index i.
        {
            while (i > 0) // While not at the root.
            {
                int p = (i - 1) / 2; // Parent index.
                if (Compare(heap[i], heap[p])) // If current node has higher priority than parent.
                {
                    var tmp = heap[i]; // Swap with parent.
                    heap[i] = heap[p]; // i now has parent's value.
                    heap[p] = tmp; // p now has current node's value.
                    i = p; // Move up to parent index.
                }
                else break; 
            }
        }

        public void SiftDown(int i) // Restore heap property by sifting down from index i.
        {
            int n = heap.Count; // Total number of elements.
            while (true) // Loop until heap property is restored.
            {
                int l = 2*i + 1; // Setting L variable
                int r = l + 1; // Setting R variable
                int smallest = i; // Assume current is smallest
                if (l < n && Compare(heap[l], heap[smallest])) smallest = l; // Check L variable
                if (r < n && Compare(heap[r], heap[smallest])) smallest = r; // Check R variable
                if (smallest == i) break; // If current is smallest, the sift down is complete.
                var tmp = heap[i]; // Swap current with smallest variable.
                heap[i] = heap[smallest]; // i now has smallest variable value.
                heap[smallest] = tmp; // smallest now has current node's value.
                i = smallest; // Move down to smallest index.
            }
        }

        public bool IsEmpty() => heap.Count == 0; // Check if the priority queue is empty.
    }
}
