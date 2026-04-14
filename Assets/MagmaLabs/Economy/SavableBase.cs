using UnityEngine;

using MagmaLabs;
namespace MagmaLabs.Economy{
    public abstract class SavableBase : ScriptableObject, Savable
    {
        public string id, name; 
        public SerializableDictionary<string> tags = new SerializableDictionary<string>();


        public virtual string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public virtual void FromString(string serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized))
            {
                return;
            }

            JsonUtility.FromJsonOverwrite(serialized, this);
            EnsureInitialized();
        }

        protected virtual void OnEnable()
        {
            EnsureInitialized();
        }

        protected virtual void OnValidate()
        {
            EnsureInitialized();
        }

        protected void EnsureInitialized()
        {
            if (tags == null)
            {
                tags = new SerializableDictionary<string>();
            }
        }
    }

}
