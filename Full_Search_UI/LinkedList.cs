using System;
using System.Collections;
using System.Collections.Generic;

namespace FullSearch
{
    // Linked list node, using T as the data type
    public class LinkedListNode<T>
    {
        public T Data { get; set; } // Data stored in the node
        public LinkedListNode<T> Next 
        {
            get; set; 
        } // Pointer to the next node
        public LinkedListNode(T data) 
        {
            Data = data; Next = null; 
        } // Constructor
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;
        public int Count { get; private set; }

        public LinkedList()
        {
            head = tail = null;
            Count = 0;
        }

        public void AddLast(T item)
        {
            var node = new LinkedListNode<T>(item);
            if (tail == null)
            {
                head = tail = node;
            }
            else
            {
                tail.Next = node;
                tail = node;
            }
            Count++;
        }

        public void AddFirst(T item)
        {
            var node = new LinkedListNode<T>(item);
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
