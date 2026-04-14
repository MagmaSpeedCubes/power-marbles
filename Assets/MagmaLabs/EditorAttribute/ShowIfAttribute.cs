using UnityEngine;
namespace MagmaLabs{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string conditionField;
        public object compareValue;

        public ShowIfAttribute(string conditionField, object value)
        {
            this.conditionField = conditionField;
            this.compareValue = value;
        }
    }

    public class ShowIfNotAttribute : ShowIfAttribute
    {
        public ShowIfNotAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfGreaterThanAttribute : ShowIfAttribute
    {
        public ShowIfGreaterThanAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfLessThanAttribute : ShowIfAttribute
    {
        public ShowIfLessThanAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfGreaterThanOrEqualAttribute : ShowIfAttribute
    {
        public ShowIfGreaterThanOrEqualAttribute(string conditionField, object value) : base(conditionField, value) { }
    }


    public class ShowIfLessThanOrEqualAttribute : ShowIfAttribute
    {
        public ShowIfLessThanOrEqualAttribute(string conditionField, object value) : base(conditionField, value) { }
    }

    public class ShowIfAnyAttribute : ShowIfAttribute
    {
        // Accepts a single field name and multiple compare values:
        // [ShowIfAny("state", State.A, State.B, State.C)]
        public string[] conditionFields;
        public object[] compareValues;

        public ShowIfAnyAttribute(string conditionField, params object[] values) : base(conditionField, values != null && values.Length > 0 ? values[0] : null)
        {
            this.conditionFields = new string[] { conditionField };
            this.compareValues = values ?? new object[0];
            // Store the full values array in base.compareValue as a fallback
            this.compareValue = this.compareValues;
        }
    }

    
}


