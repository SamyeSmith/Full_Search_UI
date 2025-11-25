using System;

namespace FullSearch
{
    // creating a stack data structure using linked list
    public class Stack<T>
    {
        private LinkedList<T> _list = new LinkedList<T>(); // new and empty linked list to form the stack

        public void Push(T item) => _list.AddFirst(item); // add item to the top of the stack

        public T Pop() // remove and return the item from the top of the stack
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Stack empty"); // check if stack is empty
            return _list.RemoveFirst(); // remove and return the first item from the linked list
        }

        public bool IsEmpty() => _list.IsEmpty(); // check if the stack is empty

        public int Count => _list.Count; // get the number of items in the stack
    }
}
