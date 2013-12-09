using System;
using System.Collections;
using System.Collections.Generic;

namespace Containers
{
    /// <summary>
    /// HashSet for Xbox360.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashSet<T> : ICollection<T>
    {
        private Dictionary<T, short> MyDict;

        public HashSet()
        {
            MyDict = new Dictionary<T, short>();
        }

        public HashSet(IEnumerable enumer)
        {
            MyDict = new Dictionary<T, short>();
            foreach (T item in enumer)
            {
                MyDict.Add(item, 0);
            }
        }
        // Methods
        public void Add(T item)
        {
            // We don't care for the value in dictionary, Keys matter.
            MyDict.Add(item, 0);
        }

        public void Clear()
        {
            MyDict.Clear();
        }

        public bool Contains(T item)
        {
            return MyDict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return MyDict.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                try
                {
                    MyDict.Add(item, 0);
                }
                catch (ArgumentException) { }
            }
        }

        // Properties
        public int Count
        {
            get { return MyDict.Keys.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}