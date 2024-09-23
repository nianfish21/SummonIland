using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tool
{

    /// <summary>
    /// ��list���ֵ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class SortMultiMap<T, K>
    {
        private readonly SortedDictionary<T, List<K>> dictionary = new SortedDictionary<T, List<K>>();

        // ����list
        private readonly Queue<List<K>> queue = new Queue<List<K>>();

        public SortedDictionary<T, List<K>> GetDictionary()
        {
            return this.dictionary;
        }
        public List<T> GetDictionaryList()
        {
            return this.dictionary.Keys.ToList();
        }

        public void Add(T t, K k)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list == null)
            {
                list = this.FetchList();
                this.dictionary[t] = list;
            }
            list.Add(k);
        }

        public KeyValuePair<T, List<K>> First()
        {
            return this.dictionary.First();
        }

        public T FirstKey()
        {
            return this.dictionary.Keys.First();
        }

        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        private List<K> FetchList()
        {
            if (this.queue.Count > 0)
            {
                List<K> list = this.queue.Dequeue();
                list.Clear();
                return list;
            }
            return new List<K>();
        }

        private void RecycleList(List<K> list)
        {
            // ��ֹ����
            if (this.queue.Count > 100)
            {
                return;
            }
            list.Clear();
            this.queue.Enqueue(list);
        }

        public bool Remove(T t, K k)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            if (!list.Remove(k))
            {
                return false;
            }
            if (list.Count == 0)
            {
                this.RecycleList(list);
                this.dictionary.Remove(t);
            }
            return true;
        }

        public bool Remove(T t)
        {
            List<K> list = null;
            this.dictionary.TryGetValue(t, out list);
            if (list != null)
            {
                this.RecycleList(list);
            }
            return this.dictionary.Remove(t);
        }

        /// <summary>
        /// �������ڲ���list,copyһ�ݳ���
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public K[] GetAll(T t)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list == null)
            {
                return new K[0];
            }
            return list.ToArray();
        }

        /// <summary>
        /// �����ڲ���list
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<K> this[T t]
        {
            get
            {
                List<K> list;
                this.dictionary.TryGetValue(t, out list);
                return list;
            }
        }

        public K GetOne(T t)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return default(K);
        }

        public bool Contains(T t, K k)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            return list.Contains(k);
        }

        public bool ContainsKey(T t)
        {
            return this.dictionary.ContainsKey(t);
        }

        public void Clear()
        {
            dictionary.Clear();
        }
    }


}