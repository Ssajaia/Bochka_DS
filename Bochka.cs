using Bochka_DS.Interface;
using System.Collections;
using System.Diagnostics;

namespace Collection.BochkaDS
{
    [DebuggerDisplay("Count = {Count}")]
    public class Bochka<T> : Ibochka<T>, IEquatable<Bochka<T>>, IComparable<Bochka<T>>
    {
        private const int DefaultCapacity = 4;
        private T[] _items;
        private int _count;
        private int _version;

        public Bochka()
        {
            _items = Array.Empty<T>();
        }

        public Bochka(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");

            _items = capacity == 0 ? Array.Empty<T>() : new T[capacity];
        }

        public Bochka(params T[] elements) : this(elements.AsSpan())
        {
        }

        public Bochka(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (collection is ICollection<T> c)
            {
                _count = c.Count;
                _items = new T[_count];
                c.CopyTo(_items, 0);
            }
            else
            {
                _items = Array.Empty<T>();
                foreach (var item in collection)
                {
                    Add(item);
                }
            }
        }

        public Bochka(ReadOnlySpan<T> elements)
        {
            _count = elements.Length;
            _items = _count == 0 ? Array.Empty<T>() : new T[_count];
            elements.CopyTo(_items);
        }

        public int Count => _count;

        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _count)
                    throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be less than current count.");

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_count > 0)
                        {
                            Array.Copy(_items, newItems, _count);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = Array.Empty<T>();
                    }
                }
            }
        }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _items[index];
            }
            set
            {
                if ((uint)index >= (uint)_count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                _items[index] = value;
                _version++;
            }
        }

        public void Add(T item)
        {
            _version++;
            if (_count == _items.Length)
            {
                EnsureCapacity(_count + 1);
            }
            _items[_count++] = item;
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void Clear()
        {
            _version++;
            if (_count > 0)
            {
                Array.Clear(_items, 0, _count);
                _count = 0;
            }
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if ((uint)arrayIndex > (uint)array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < _count)
                throw new ArgumentException("Destination array is not long enough.");

            Array.Copy(_items, 0, array, arrayIndex, _count);
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => Array.IndexOf(_items, item, 0, _count);

        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _version++;
            if (_count == _items.Length)
            {
                EnsureCapacity(_count + 1);
            }
            if (index < _count)
            {
                Array.Copy(_items, index, _items, index + 1, _count - index);
            }
            _items[index] = item;
            _count++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_count)
                throw new ArgumentOutOfRangeException(nameof(index));

            _version++;
            _count--;
            if (index < _count)
            {
                Array.Copy(_items, index + 1, _items, index, _count - index);
            }
            _items[_count] = default!;
        }

        public string Stringify()
        {
            if (_count == 0)
                return string.Empty;

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < _count; i++)
            {
                sb.Append(_items[i]?.ToString() ?? "null");
                if (i < _count - 1)
                    sb.Append(' ');
            }
            return sb.ToString();
        }

        public void Print() => Console.WriteLine(Stringify());

        public void TrimExcess()
        {
            int threshold = (int)(_items.Length * 0.9);
            if (_count < threshold)
            {
                Capacity = _count;
            }
        }

        #region Operator Overloads
        public static bool operator ==(Bochka<T>? left, Bochka<T>? right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if (left is null || right is null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Bochka<T>? left, Bochka<T>? right) => !(left == right);

        public static bool operator <(Bochka<T> left, Bochka<T> right)
        {
            if (left is null)
                return right is not null;

            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Bochka<T> left, Bochka<T> right)
        {
            if (left is null)
                return false;

            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Bochka<T> left, Bochka<T> right) => !(left > right);
        public static bool operator >=(Bochka<T> left, Bochka<T> right) => !(left < right);
        #endregion

        #region Equality and Comparison
        public override bool Equals(object? obj) => Equals(obj as Bochka<T>);

        public bool Equals(Bochka<T>? other)
        {
            if (other is null || _count != other._count)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _count; i++)
            {
                if (!comparer.Equals(_items[i], other._items[i]))
                    return false;
            }
            return true;
        }

        public int CompareTo(Bochka<T>? other)
        {
            if (other is null)
                return 1;

            var comparer = Comparer<T>.Default;
            int minLength = Math.Min(_count, other._count);
            for (int i = 0; i < minLength; i++)
            {
                int result = comparer.Compare(_items[i], other._items[i]);
                if (result != 0)
                    return result;
            }
            return _count.CompareTo(other._count);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            for (int i = 0; i < _count; i++)
            {
                hashCode.Add(_items[i]);
            }
            return hashCode.ToHashCode();
        }
        #endregion

        #region Enumerator
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly Bochka<T> _bochka;
            private readonly int _version;
            private int _index;
            private T? _current;

            internal Enumerator(Bochka<T> bochka)
            {
                _bochka = bochka;
                _version = bochka._version;
                _index = 0;
                _current = default;
            }

            public T Current => _current!;

            object? IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index > _bochka._count)
                        throw new InvalidOperationException();

                    return Current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _bochka._version)
                    throw new InvalidOperationException("Collection was modified during enumeration.");

                if (_index < _bochka._count)
                {
                    _current = _bochka._items[_index];
                    _index++;
                    return true;
                }
                _current = default;
                return false;
            }

            public void Reset()
            {
                if (_version != _bochka._version)
                    throw new InvalidOperationException("Collection was modified during enumeration.");

                _index = 0;
                _current = default;
            }
        }
        #endregion
    }
}