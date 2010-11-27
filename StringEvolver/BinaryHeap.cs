using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver
{
    class BinaryNodeValue<T, T1> where T : IComparable<T>
    {
        public T Key { get; set; }
        public T1 Value { get; set; }

        public BinaryNodeValue(T key, T1 value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Key, Value);
        }
    }

    class BinaryHeap<T, T1> where T : IComparable<T>
    {
        public int Count
        {
            get
            {
                return items.Count;
            }
        }
        private readonly List<BinaryNodeValue<T, T1>> items;

        public BinaryHeap() : this(100) { }

        public BinaryHeap(int initialCapacity)
        {
            items = new List<BinaryNodeValue<T, T1>>(initialCapacity);
        }

        public void Insert(T priority, T1 item)
        {
            var node = new BinaryNodeValue<T, T1>(priority, item);
            items.Add(node);
            Upheap(items.Count - 1);
        }

        public T1 RemoveTop()
        {
            var minValue = items[0].Value;
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            Downheap(0);
            return minValue;
        }

        public T1 PeakTop()
        {
            return items[0] == null ? default(T1) : items[0].Value;
        }

        void Upheap(int rank)
        {
            int parentRank = ParentRank(rank);
            if (parentRank < 0)
            {
                return;
            }
            BinaryNodeValue<T, T1> parent = items.ElementAt(parentRank), node = items.ElementAt(rank);
            if (parent.Key.CompareTo(node.Key) <= 0)
            {
                return;
            }
            Swap(rank, parentRank);
            Upheap(parentRank);
        }

        void Downheap(int rank)
        {
            int leftChildRank = LeftChildRank(rank),
                rightChildRank = RightChildRank(rank),
                minimumChildRank = MinimumRank(leftChildRank, rightChildRank);
            if (minimumChildRank > items.Count - 1 || items[minimumChildRank].Key.CompareTo(items[rank].Key) >= 0)
            {
                return;
            }
            Swap(rank, minimumChildRank);
            Downheap(minimumChildRank);
        }

        static int ParentRank(int rank)
        {
            return (int)Math.Floor(((float)(rank - 1) / 2));
        }

        static int LeftChildRank(int rank)
        {
            return 2 * rank + 1;
        }

        static int RightChildRank(int rank)
        {
            return LeftChildRank(rank) + 1;
        }

        void Swap(int rank1, int rank2)
        {
            var temp = items[rank1];
            items[rank1] = items[rank2];
            items[rank2] = temp;
        }

        int MinimumRank(int rank1, int rank2)
        {
            int top = items.Count - 1;
            if (rank1 > top || rank2 > top)
            {
                return rank1 > top ? rank2 : rank1;
            }
            return items[rank1].Key.CompareTo(items[rank2].Key) < 0 ? rank1 : rank2;
        }
    }

}
