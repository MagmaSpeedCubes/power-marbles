using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A sparse list: elements stored by integer index with no requirement for consecutive indices.
/// Inserting at index 100 does not shift other elements. Use for dynamic/optional indices.
/// </summary>
/// 

[System.Serializable]

public class SparseList<T> : IEnumerable<T>
{
    private Dictionary<int, T> _items = new Dictionary<int, T>();

    /// <summary>
    /// Add or replace element at the given index.
    /// </summary>
    public void Set(int index, T item)
    {
        if (index < 0) throw new ArgumentException("Index must be >= 0", nameof(index));
        _items[index] = item;
    }

    /// <summary>
    /// Get element at index, or default(T) if not present.
    /// </summary>
    public T Get(int index)
    {
        return _items.TryGetValue(index, out var item) ? item : default;
    }

    /// <summary>
    /// Check if element exists at index.
    /// </summary>
    public bool Contains(int index)
    {
        return _items.ContainsKey(index);
    }

    /// <summary>
    /// Remove element at index.
    /// </summary>
    public bool Remove(int index)
    {
        return _items.Remove(index);
    }

    /// <summary>
    /// Number of items currently stored.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Get all stored indices (in sorted order).
    /// </summary>
    public IEnumerable<int> Indices => _items.Keys.OrderBy(k => k);

    /// <summary>
    /// Indexer for convenient access: list[5] = ball;
    /// </summary>
    public T this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }

    /// <summary>
    /// Enumerate all items in index order.
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
        return _items.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Clear all items.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }
}