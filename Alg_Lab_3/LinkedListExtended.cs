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

        tail = head; // Старый первый элемент станет последним

        while (current != null)
        {
            nextTemp = current.Next; // Запоминаем следующий
            current.Next = prev; // Разворачиваем ссылку
            prev = current; // Двигаем prev
            current = nextTemp; // Двигаем current
        }

        head = prev; // Новый первый элемент
    }

    // 2.1. Перенос последнего элемента в начало
    public void MoveLastToFront()
    {
        if (head == null || head == tail) return;

        Node<T>? current = head;

        // Ищем предпоследний элемент, так как у нас нет ссылки Prev
        while (current.Next != tail)
        {
            current = current.Next;
        }

        Node<T>? oldTail = tail;

        current.Next = null; // Отвязываем последний
        tail = current; // Новый хвост - предпоследний
        oldTail.Next = head; // Старый хвост указывает на старую голову
        head = oldTail; // Старый хвост становится новой головой
    }

    // 2.2. Перенос первого элемента в конец
    public void MoveFirstToEnd()
    {
        if (head == null || head == tail) return;

        Node<T>? oldHead = head;

        head = oldHead.Next; // Сдвигаем голову на второй элемент
        oldHead.Next = null; // Отвязываем старый первый

        tail.Next = oldHead; // Привязываем его после хвоста
        tail = oldHead; // Обновляем хвост
    }

    // 3. Определить количество различных элементов (для целых чисел)
    public int CountDistinct()
    {
        if (head == null) return 0;

        // HashSet сам отбрасывает дубликаты. 
        // Мы просто бежим по списку и кидаем туда значения.
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

        // 1. Считаем, сколько раз встречается каждый элемент
        var counts = new Dictionary<T, int>();
        var current = head;

        while (current != null)
        {
            if (!counts.ContainsKey(current.Value))
                counts[current.Value] = 0;

            counts[current.Value]++;
            current = current.Next;
        }

        // 2. Проходим второй раз и удаляем те, у которых Count > 1
        Node<T>? prev = null;
        current = head;

        while (current != null)
        {
            if (counts[current.Value] > 1) // Если дубликат
            {
                if (prev == null)
                {
                    head = current.Next; // Удаляем первый элемент
                }
                else
                {
                    prev.Next = current.Next; // "Перепрыгиваем" через элемент
                }

                if (current == tail) // Если удалили последний
                {
                    tail = prev;
                }

                Count--;
            }
            else
            {
                // ВАЖНО: prev сдвигается только если мы НЕ удалили узел
                prev = current;
            }

            current = current.Next;
        }
    }

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