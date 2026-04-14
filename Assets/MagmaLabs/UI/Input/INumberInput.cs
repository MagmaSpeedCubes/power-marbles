using UnityEngine;
using UnityEngine.Events;

namespace MagmaLabs.UI{
    public interface INumberInput
    {
        public UnityEvent<float> OnValueChanged { get; }
    }

    public abstract class NumberInputBase : MonoBehaviour, INumberInput
    {
        public float inputValue { get; protected set; }
        public virtual UnityEvent<float> OnValueChanged { get; protected set; } = new UnityEvent<float>();

    }
}