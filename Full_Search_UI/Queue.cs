using System;

namespace FullSearch
{
    public class Queue<T>
    {
        private LinkedList<T> _list = new LinkedList<T>();

        public void Enqueue(T item) => _list.AddLast(item);

        public T Dequeue()
        {
            if (_list.IsEmpty()) throw new InvalidOperationException("Queue empty");
            return _list.RemoveFirst();
        }

        public bool IsEmpty() => _list.IsEmpty();

        public int Count => _list.Count;
    }
}
