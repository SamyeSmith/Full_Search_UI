using System;

namespace FullSearch
{
    public class Stack<T>
    {
        private LinkedList<T> _list = new LinkedList<T>();

        public void Push(T item) => _list.AddFirst(item);

        public T Pop()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Stack empty");
            return _list.RemoveFirst();
        }

        public bool IsEmpty() => _list.IsEmpty();

        public int Count => _list.Count;
    }
}
