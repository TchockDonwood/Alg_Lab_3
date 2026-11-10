namespace Alg_Lab_3;

public class LinkedListExtended<T> : LinkedList<T>

{
    // 1. Написать функцию, которая переворачивает список L
    public void Reverse()
    {
        if (head == null || head.Next == null) return;

        Node<T>? prev = null;
        Node<T>? current = head;
        Node<T>? nextTemp = null;

        tail = head;

        while (current != null)
        {
            nextTemp = current.Next;
            current.Next = prev;
            prev = current;
            current = nextTemp;
        }

        head = prev;
    }

    // 2.1. Перенос последнего элемента в начало
    public void MoveLastToFront()
    {
        if (head == null || head == tail) return;

        Node<T>? current = head;

        while (current.Next != tail)
        {
            current = current.Next;
        }

        Node<T>? oldTail = tail;

        current.Next = null;
        tail = current;
        oldTail.Next = head;
        head = oldTail;
    }

    // 2.2. Перенос первого элемента в конец
    public void MoveFirstToEnd()
    {
        if (head == null || head == tail) return;

        Node<T>? oldHead = head;

        head = oldHead.Next;
        oldHead.Next = null;

        tail.Next = oldHead;
        tail = oldHead;
    }

    // 3. Определить количество различных элементов (для целых чисел)
    public int CountDistinct()
    {
        if (head == null) return 0;

        HashSet<T> unique = new HashSet<T>();
        Node<T>? current = head;

        while (current != null)
        {
            unique.Add(current.Value);
            current = current.Next;
        }

        return unique.Count;
    }

    // 4. Удалить из списка неуникальные элементы (оставить только те, что встречаются 1 раз)
    public void RemoveNonUnique()
    {
        if (head == null) return;

        var counts = new Dictionary<T, int>();
        var current = head;

        while (current != null)
        {
            if (!counts.ContainsKey(current.Value))
                counts[current.Value] = 0;

            counts[current.Value]++;
            current = current.Next;
        }

        Node<T>? prev = null;
        current = head;

        while (current != null)
        {
            if (counts[current.Value] > 1)
            {
                if (prev == null)
                {
                    head = current.Next;
                }
                else
                {
                    prev.Next = current.Next;
                }

                if (current == tail)
                {
                    tail = prev;
                }

                Count--;
            }
            else
            {
                prev = current;
            }

            current = current.Next;
        }
    }

    // 5. Вставка списка в себя после первого вхождения x
    public bool InsertListAfterFirstOccurrence(LinkedList<T> list, T target)
    {
        if (list is null || list.head is null || head is null) return false;

        var cmp = EqualityComparer<T>.Default;

        var current = head;
        while (current != null && !cmp.Equals(current.Value, target))
            current = current.Next;

        if (current is null) return false;

        var copy = list.Clone();

        var temp = current.Next;
        current.Next = copy.head;
        copy.tail!.Next = temp;

        if (temp is null)
            tail = copy.tail;

        Count += copy.Count;
        return true;
    }

    // 6. Вставка элемента в упорядоченный список
    public void InsertInSortedList(T item)
    {
        var node = new Node<T>(item);
        var cmp = Comparer<T>.Default;

        if (head == null)
        {
            head = node;
            tail = node;
            Count = 1;
            return;
        }

        if (cmp.Compare(item, head.Value) <= 0)
        {
            node.Next = head;
            head = node;
            Count++;
            return;
        }

        var current = head;
        while (current.Next != null && cmp.Compare(current.Next.Value, item) < 0)
            current = current.Next;

        node.Next = current.Next;
        current.Next = node;

        if (node.Next == null)
            tail = node;

        Count++;
    }

    // 7. Удаление всех вхождений элемента E
    // LinkedList<T>.RemoveAll();

    // 8. Вставка элемента F перед первым вхождением E
    public bool InsertBeforeFirstOccurrence(T target, T item)
    {
        if (head == null || target is null) return false;

        var cmp = EqualityComparer<T>.Default;

        if (cmp.Equals(head.Value, target))
        {
            var temp = new Node<T>(item);
            temp.Next = head;
            head = temp;
            Count++;
            return true;
        }

        for (var prev = head; prev.Next != null; prev = prev.Next)
        {
            if (cmp.Equals(prev.Next.Value, target))
            {
                var temp = new Node<T>(item);
                temp.Next = prev.Next;
                prev.Next = temp;
                Count++;
                return true;
            }
        }

        return false;
    }

    // 9. Дописывание списка E к списку L
    public void AppendList(LinkedList<T> list)
    {
        if (list is null || ReferenceEquals(this, list) || list.head is null)
            return;

        var copy = list.Clone();

        if (head is null)
        {
            head = copy.head;
            tail = copy.tail;
            Count = copy.Count;
        }
        else
        {
            tail!.Next = copy.head;
            tail = copy.tail;
            Count += copy.Count;
        }
    }

    // 10. Разбиение списка на два по первому вхождению заданного числа
    public LinkedList<T> SplitList(T target)
    {
        if (head == null || target is null) return new LinkedList<T>();

        var cmp = EqualityComparer<T>.Default;

        if (cmp.Equals(head.Value, target))
        {
            var copy = this.Clone();
            this.Clear();
            return copy;
        }

        var index = 0;
        for (var prev = head; prev.Next != null; prev = prev.Next)
        {
            index++;
            if (cmp.Equals(prev.Next.Value, target))
            {
                var copy = this.Clone(index, Count - 1);
                prev.Next = null;
                tail = prev;
                Count = index;
                return copy;
            }
        }

        return new LinkedList<T>();
    }

    // 11. Удвоение списка (приписывание себя к концу)
    public void DoubleList()
    {
        if (head == null) return;

        var copy = this.Clone();

        tail!.Next = copy.head;
        tail = copy.tail;
        Count += copy.Count;
    }

    // 12. Поменять местами два элемента списка
    public bool SwapElements(T first, T second)
    {
        if (head == null) return false;

        var cmp = EqualityComparer<T>.Default;

        Node<T>? firstNode = null;
        Node<T>? secondNode = null;

        for (var cur = head; cur != null; cur = cur.Next)
        {
            if (firstNode == null && cmp.Equals(cur.Value, first))
            {
                firstNode = cur;
            }
            else if (secondNode == null && cmp.Equals(cur.Value, second))
            {
                secondNode = cur;
            }

            if (firstNode != null && secondNode != null) break;
        }

        if (firstNode == null || secondNode == null) return false;

        (firstNode.Value, secondNode.Value) = (secondNode.Value, firstNode.Value);
        return true;
    }
}