using System;
using System.Collections;
using System.Collections.Generic;

namespace FullSearch
{
    // Linked list node, using T as the data type
    public class LinkedListNode<T> // This is the element equivilent
    {
        public T Data { get; set; } // Data stored in the node
        // Pointer to the next node
        public LinkedListNode<T> Next 
        {
            get; set; 
        }
        // Constructor to initialize the node with data
        public LinkedListNode(T data) 
        {
            Data = data; Next = null; 
        }
    }

    // Linked list class implementing IEnumerable for iteration
    public class LinkedList<T> : IEnumerable<T>
    {
        private LinkedListNode<T> head; // Pointer to the first node
        private LinkedListNode<T> tail; // Pointer to the last node
        public int Count { get; private set; } // Number of elements in the list

        // Constructor to initialize an empty list
        public LinkedList()
        {
            head = tail = null;
            Count = 0;
        }

        // Add an item to the end of the list
        public void AddLast(T item)
        {
            var node = new LinkedListNode<T>(item); // Create a new node
            if (tail == null) // If the list is empty
            {
                head = tail = node; // Set both head and tail to the new node
            }
            else // If the list is not empty
            {
                tail.Next = node; // Link the new node at the end
                tail = node; // Update the tail to the new node
            }
            Count++; // Increment the count
        }

        public void AddFirst(T item) // Add an item to the beginning of the list
        {
            var node = new LinkedListNode<T>(item); // Create a new node
            if (head == null)
            {
                head = tail = node;
            }
            else
            {
                node.Next = head;
                head = node;
            }
            Count++;
        }

        public T RemoveFirst()
        {
            if (head == null) throw new InvalidOperationException("List empty");
            var val = head.Data;
            head = head.Next;
            if (head == null) tail = null;
            Count--;
            return val;
        }

        public bool IsEmpty() => Count == 0;

        public IEnumerator<T> GetEnumerator()
        {
            var cur = head;
            while (cur != null)
            {
                yield return cur.Data;
                cur = cur.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
