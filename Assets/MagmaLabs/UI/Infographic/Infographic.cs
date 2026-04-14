using System;

using UnityEngine;
using UnityEngine.Events;

using MagmaLabs;


namespace MagmaLabs.UI{

    public interface IInfographic
    {
        void SetValue(float value);
        void SetRange(float min, float max);                      

        void Refresh();
        void SetColor(Color color);                              

        // float currentValue;
        // Range<float> valueRange;


    }


    public interface IInfographicEnhanced : IInfographic
    {
                  
        void SetAnimationCurve(AnimationCurve curve); 
        void AnimateToValue(float targetValue, float duration); 
        void StopAnimation();
        void SetPrecision(int decimals);

        void HandleOverflow();


    }
    [System.Serializable]
    public abstract class InfographicBase : MonoBehaviour, IInfographic
    {
        public float currentValue;
        public Range<float> valueRange;
        

        public virtual void SetValue(float value)
        {
            currentValue = value;
            Refresh();
        }

        public virtual void SetRange(float min, float max)
        {
            valueRange = new Range<float>() { min = min, max = max };
        }

        public virtual float GetPercentage()
        {
            if (valueRange.max - valueRange.min == 0) return 0f;
            return (currentValue - valueRange.min) / (valueRange.max - valueRange.min);
        }
        public virtual void SetColor(Color color) { }
        public virtual void Refresh() { }

    }
    [System.Serializable]
    public abstract class InfographicBaseEnhanced : InfographicBase, IInfographicEnhanced
    {
        public virtual AnimationCurve animationCurve{get; protected set;}
        public int precision { get; protected set; }
        public virtual OverflowBehaviour overflowBehaviour { get; protected set; }
        
        public UnityEvent<string> OnValueChanged;
        public UnityEvent OnAnimationComplete;
        public virtual void SetColor(Color color) { }
        public virtual void SetAnimationCurve(AnimationCurve curve)
        {
            animationCurve = curve;
        }
        public virtual void AnimateToValue(float targetValue, float duration) { }
        public virtual void StopAnimation() { }
        public virtual void SetPrecision(int decimals)
        {
            precision = decimals;
        }

        public virtual void HandleOverflow(){}

    }

    public enum OverflowBehaviour
    {
        Clamp,
        Loop, 
        Overload
    }
}
