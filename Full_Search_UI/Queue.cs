using System;

namespace FullSearch
{
    // using linked list to implement a queue data structure.
    public class Queue<T>
    {
        private LinkedList<T> _list = new LinkedList<T>(); // Store queue elements using internal and private linked list.

        public void Enqueue(T item) => _list.AddLast(item); // Add item to the end of the queue.

        public T Dequeue() // Remove and return the item at the front of the queue.
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Queue empty"); // Check if the queue is empty before dequeuing.
            return _list.RemoveFirst(); // Remove and return the first item from the linked list.
        }

        public bool IsEmpty() => _list.IsEmpty(); // Check if the queue is empty.

        public int Count => _list.Count; // Get the number of items in the queue.
    }
}
