using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;

namespace MagmaLabs{

[System.Serializable]
public struct BoundedValue<T> where T : ISerializable
    {
        /// <summary>
        /// A value of type T that is constrained to be between min and max. The type T must support comparison operators and be serializable by Unity. The min and max values are inclusive. If the value is set to something outside the bounds, it will be clamped to the nearest bound. This struct is useful for things like health, mana, or any other stat that has a minimum and maximum value. It can also be used for things like progress bars or sliders.
        /// </summary>
        public T value;
        public Range<float> range;

        public BoundedValue(T value, float min, float max)
        {

            this.value = value;
            range = new Range<float>() { min = min, max = max };
        }

        public BoundedValue(T value, Range<float> range)
        {
            this.value = value;
            this.range = range;
        }



    }

}


// Source - https://stackoverflow.com/a/5343033
// Posted by drharris, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-20, License - CC BY-SA 3.0

/// <summary>The Range class.</summary>
/// <typeparam name="T">Generic parameter.</typeparam>
[System.Serializable]
public class Range<T> where T : IComparable<T>
{
    /// <summary>min value of the range.</summary>
    public T min;

    /// <summary>max value of the range.</summary>
    public T max;

        /// <summary>
        /// Numeric length of the range (max - min) converted to double.
        /// This avoids using `dynamic` which Unity's runtime doesn't support.
        /// </summary>
        public double length
        {
            get
            {
                // use Convert.ToDouble, will work for any IConvertible numeric type
                double a = Convert.ToDouble(max);
                double b = Convert.ToDouble(min);
                return a - b;
            }
        }
        
    

    /// <summary>Presents the Range in readable format.</summary>
    /// <returns>String representation of the Range</returns>
    public override string ToString()
    {
        return string.Format("[{0} - {1}]", this.min, this.max);
    }

    /// <summary>Determines if the range is valid.</summary>
    /// <returns>True if range is valid, else false</returns>
    public bool IsValid()
    {
        return this.min.CompareTo(this.max) <= 0;
    }

    /// <summary>Determines if the provided value is inside the range.</summary>
    /// <param name="value">The value to test</param>
    /// <returns>True if the value is inside Range, else false</returns>
    public bool ContainsValue(T value)
    {
        return (this.min.CompareTo(value) <= 0) && (value.CompareTo(this.max) <= 0);
    }

    /// <summary>Determines if this Range is inside the bounds of another range.</summary>
    /// <param name="Range">The parent range to test on</param>
    /// <returns>True if range is inclusive, else false</returns>
    public bool IsInsideRange(Range<T> range)
    {
        return this.IsValid() && range.IsValid() && range.ContainsValue(this.min) && range.ContainsValue(this.max);
    }

    /// <summary>Determines if another range is inside the bounds of this range.</summary>
    /// <param name="Range">The child range to test</param>
    /// <returns>True if range is inside, else false</returns>
    public bool ContainsRange(Range<T> range)
    {
        return this.IsValid() && range.IsValid() && this.ContainsValue(range.min) && this.ContainsValue(range.max);
    }


}
