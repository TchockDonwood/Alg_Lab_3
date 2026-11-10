using System.Collections;

namespace Alg_Lab_3;

public class LinkedList<T> : IEnumerable<T>
{
    public Node<T>? head { get; internal set; }
    public Node<T>? tail { get; internal set; }
    public int Count { get; internal set; }

    public LinkedList() { }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var current = head!;
            for (int i = 0; i < index; i++)
                current = current.Next!;

            return current.Value;
        }
        set
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var current = head!;
            for (int i = 0; i < index; i++)
                current = current.Next!;

            current.Value = value;
        }
    }
    public IEnumerator<T> GetEnumerator() => new ListEnumerator<T>(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item)
    {
        var node = new Node<T>(item);

        if (head == null)
        {
            head = node;
            tail = node;
        }
        else
        {
            tail!.Next = node;
            tail = node;
        }
        Count++;
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
        {
            var prev = head;
            head = new Node<T>(item);
            head!.Next = prev;

            if (tail == null)
                tail = head;
        }
        else
        {
            var prev = head!;
            for (int i = 0; i < index - 1; i++)
                prev = prev.Next!;

            var temp = new Node<T>(item);
            temp.Next = prev.Next;
            prev.Next = temp;

            if (temp.Next == null)
                tail = temp;
        }
        Count++;
    }

    public bool Contains(T item)
    {
        if (head == null) return false;

        var cmp = EqualityComparer<T>.Default;
        for (var cur = head; cur != null; cur = cur.Next)
            if (cmp.Equals(cur.Value, item))
                return true;

        return false;
    }

    public bool Remove(T item)
    {
        if (head == null) return false;

        var cmp = EqualityComparer<T>.Default;

        if (cmp.Equals(head.Value, item))
        {
            head = head.Next;
            Count--;
            if (Count == 0) tail = null;
            return true;
        }

        var prev = head;
        while (prev.Next != null)
        {
            if (cmp.Equals(prev.Next.Value, item))
            {
                var removed = prev.Next;
                prev.Next = removed.Next;
                Count--;

                if (removed.Next == null)
                    tail = prev;

                return true;
            }
            prev = prev.Next;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
        {
            head = head!.Next;
        }
        else
        {
            var prev = head!;
            for (int i = 0; i < index - 1; i++)
                prev = prev.Next!;

            var removed = prev.Next!;
            prev.Next = removed.Next;

            if (removed.Next == null)
                tail = prev;
        }
        Count--;

        if (Count == 0)
            tail = null;
    }

    public int RemoveAll(T item)
    {
        var cmp = EqualityComparer<T>.Default;
        int removedItems = 0;

        while (head != null && cmp.Equals(head.Value, item))
        {
            head = head.Next;
            removedItems++;
        }

        if (head == null)
        {
            Count -= removedItems;
            tail = null;
            return removedItems;
        }

        var prev = head;
        while (prev.Next != null)
        {
            if (cmp.Equals(prev.Next.Value, item))
            {
                var removed = prev.Next;
                prev.Next = removed.Next;
                removedItems++;
            }
            else
            {
                prev = prev.Next;
            }
        }

        tail = prev;
        Count -= removedItems;
        return removedItems;
    }

    public LinkedList<T> Clone()
    {
        var copy = new LinkedList<T>();
        for (var cur = head; cur != null; cur = cur.Next)
            copy.Add(cur.Value);
        return copy;
    }

    public LinkedList<T> Clone(int start, int end)
    {
        if (start < 0 || start >= Count) throw new ArgumentOutOfRangeException(nameof(start));
        if (end < 0 || end >= Count)     throw new ArgumentOutOfRangeException(nameof(end));
        if (start > end)                 throw new ArgumentException("start must be <= end", nameof(start));

        var copy = new LinkedList<T>();
        var cur = head;
        int index = 0;
        while (cur != null && index <= end)
        {
            if (index >= start)
                copy.Add(cur.Value);
            cur = cur.Next;
            index++;
        }
        return copy;
    }

    public void Clear()
    {
        head = null;
        tail = null;
        Count = 0;
    }

    private sealed class ListEnumerator<TItem> : IEnumerator<TItem>
    {
        private readonly LinkedList<TItem> _list;
        private Node<TItem>? _current;
        private bool _started;
        private bool _finished;

        public ListEnumerator(LinkedList<TItem> list) => _list = list;

        public TItem Current
        {
            get
            {
                if (!_started || _finished)
                    throw new InvalidOperationException(
                        "Enumeration has either not started or has already finished.");
                return _current!.Value;
            }
        }

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_finished) return false;
            _started = true;
            _current = _current is null ? _list.head : _current.Next;
            if (_current is null)
            {
                _finished = true;
                return false;
            }
            return true;
        }

        public void Reset()
        {
            _current = null;
            _started = false;
            _finished = false;
        }

        public void Dispose() { }
    }
}