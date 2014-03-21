using System;
using System.Collections;
using System.Collections.Generic;

namespace Containers
{
    /// <summary>
    /// HashSet for Xbox 360 functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashSet<T> : ICollection<T>
    {
        private Dictionary<T, short> mDict;

        public HashSet()
        {
            this.mDict = new Dictionary<T, short>();
        }

        public HashSet(HashSet<T> from)
        {
            this.mDict = new Dictionary<T, short>();
            foreach (T n in from)
                this.mDict.Add(n, 0);
        }

        #region "Methods"
        public void Add(T item)
        {
            this.mDict.Add(item, 0);
        }

        public void Clear()
        {
            this.mDict.Clear();
        }

        public bool Contains(T item)
        {
            return this.mDict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var _item in this.mDict.Keys)
                array[arrayIndex++] = _item;
        }

        public bool Remove(T item)
        {
            return this.mDict.Remove(item);
        }

        public IEnumerator GetEnumerator()
        {
            return this.mDict.Keys.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.mDict.Keys.GetEnumerator();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                try
                {
                    this.mDict.Add(item, 0);
                }
                catch (ArgumentException) { }
            }
        }
        #endregion

        #region "Properties"
        public int Count
        {
            get { return this.mDict.Keys.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}