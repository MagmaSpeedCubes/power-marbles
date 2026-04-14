using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MagmaLabs
{
    [Serializable]
    public class BoundedDictionary<T> where T : ISerializable
    {
        [Serializable]
        private class Entry
        {
            public BoundedValue<T> boundedValue;

            public Entry(BoundedValue<T> boundedValue)
            {
                this.boundedValue = boundedValue;
            }
        }

        private readonly List<Entry> entries = new List<Entry>();

        public int Count => entries.Count;

        public void Clear()
        {
            entries.Clear();
        }

        public void Add(T value, float min, float max)
        {
            Add(new BoundedValue<T>(value, min, max));
        }

        public void Add(BoundedValue<T> boundedValue)
        {
            entries.Add(new Entry(boundedValue));
        }

        public bool Remove(T value)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(entries[i].boundedValue.value, value))
                {
                    entries.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool TryGetAll(float value, List<T> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }

            results.Clear();

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].boundedValue.range.ContainsValue(value))
                {
                    results.Add(entries[i].boundedValue.value);
                }
            }

            return results.Count > 0;
        }

        public List<T> GetAll(float value)
        {
            var results = new List<T>();
            TryGetAll(value, results);
            return results;
        }

        public IEnumerable<BoundedValue<T>> AllEntries()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                yield return entries[i].boundedValue;
            }
        }
    }
}
