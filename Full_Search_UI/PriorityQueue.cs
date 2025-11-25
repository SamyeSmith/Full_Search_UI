using System;
using System.Collections.Generic;

namespace FullSearch
{
    public class PriorityQueue
    {
        private List<SearchNode> heap = new List<SearchNode>();
        public int Count => heap.Count;

        private bool Compare(SearchNode a, SearchNode b)
        {
            if (a.FCost != b.FCost) return a.FCost < b.FCost;
            return a.HCost < b.HCost;
        }

        public void Enqueue(SearchNode node)
        {
            heap.Add(node);
            SiftUp(heap.Count - 1);
        }

        public SearchNode Dequeue()
        {
            if (heap.Count == 0) throw new InvalidOperationException("PriorityQueue empty");
            var root = heap[0];
            var last = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 0)
            {
                heap[0] = last;
                SiftDown(0);
            }
            return root;
        }

        public void SiftUp(int i)
        {
            while (i > 0)
            {
                int p = (i - 1) / 2;
                if (Compare(heap[i], heap[p]))
                {
                    var tmp = heap[i];
                    heap[i] = heap[p];
                    heap[p] = tmp;
                    i = p;
                }
                else break;
            }
        }

        public void SiftDown(int i)
        {
            int n = heap.Count;
            while (true)
            {
                int l = 2*i + 1;
                int r = l + 1;
                int smallest = i;
                if (l < n && Compare(heap[l], heap[smallest])) smallest = l;
                if (r < n && Compare(heap[r], heap[smallest])) smallest = r;
                if (smallest == i) break;
                var tmp = heap[i];
                heap[i] = heap[smallest];
                heap[smallest] = tmp;
                i = smallest;
            }
        }

        public bool IsEmpty() => heap.Count == 0;
    }
}
