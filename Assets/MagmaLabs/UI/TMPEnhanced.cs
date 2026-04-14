using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

using MagmaLabs.Animation;
using MagmaLabs.Utilities;
using MagmaLabs;

namespace MagmaLabs.UI{
    [CreateAssetMenu(fileName = "TMPEnhanced", menuName = "MagmaLabs/UI/TMPEnhanced", order = 1)]

public class TMPEnhanced : TextMeshProUGUI
    {
        private string fullText = "";
        private int writeOn = 0;
        private bool writeActive = false;
        private AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);

        public UnityEvent<string> OnValueChanged;
        public UnityEvent OnAnimationComplete;

        void Start()
        {
            fullText = base.text;
            writeOn = fullText.Length;
        }

        private void Refresh()
        {
            base.text = fullText.Substring(0, writeOn);
        }
        
        public void SetColor(Color newColor)
        {
            color = newColor;
        }
        public Color GetColor()
        {
            return color;
        }
        public IEnumerator FadeColor(Color targetColor, float duration)
        {
            Color startColor = color;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                Color newColor = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                color = newColor;
                yield return null;
            }
        }
        public IEnumerator FadeText(float targetAlpha, float duration)
        {
            yield return StartCoroutine(FadeColor(new Color(color.r, color.g, color.b, targetAlpha), duration));
        }
        public IEnumerator FadeIn(float duration)
        {
            yield return StartCoroutine(FadeText(duration, 1f));
        }
        public IEnumerator FadeOut(float duration)
        {
            yield return StartCoroutine(FadeText(duration, 0f));
        }
        public void SetText(string text)
        {
            fullText = text;
            writeOn = fullText.Length;
            Refresh();
        }

        public void SetHiddenText(string text)
        {
            SetText(text);
            HideText();
        }
        public void AddText(string text)
        {
            fullText += text;
            writeOn += text.Length;
            Refresh();
        }

        public void AddHiddenText(string text)
        {
            fullText += text;
            Refresh();
        }

        public void HideText()
        {
            writeOn = 0;
            Refresh();
        }

        public void ClearText()
        {
            fullText = "";
            writeOn = 0;
            Refresh();
        }

        public string GetFullText()
        {
            return fullText;
        }

        public string GetDisplayedText()
        {
            return base.text;
        }

        public void SetWriteOn(float nw)
        {
            writeOn = (int)Mathf.Clamp(nw, 0, fullText.Length);
            Refresh();
        }

        public void SetWriteOnNormalized(float nw)
        {
            SetWriteOn((int)(nw * fullText.Length));
        }

        public float GetWriteOn()
        {
            return writeOn;
        }

        public float GetWriteOnNormalized()
        {
            if(writeOn==fullText.Length)
            {
                return 1f;
            }
            return (float)writeOn / fullText.Length;
        }

        public IEnumerator WriteOn(float targetWriteOn, float duration)
        {
            if(writeActive)
            {
                yield break;
            }
            writeActive = true;

            float startWriteOn = writeOn;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newWriteOn = Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration);
                SetWriteOn(newWriteOn);
                yield return null;
            }
            writeOn = (int)targetWriteOn;
            Refresh();
            writeActive = false;
        }

        public IEnumerator WriteOnNormalized(float targetWriteOn, float duration)
        {
            if(writeActive)
            {
                yield break;
            }
            writeActive = true;

            float startWriteOn = writeOn;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newWriteOn = Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration);
                SetWriteOnNormalized(newWriteOn);
                yield return null;
            }
            SetWriteOnNormalized(targetWriteOn);
            Refresh();
            writeActive = false;
        }

        /// <summary>
        /// Writes the next line over the given duration showing each character at the end of its interval.
        /// </summary>
        /// <param name="duration">The duration of the write on</param>
        /// <returns></returns>
        public IEnumerator WriteLine(float duration)
        {
            if(writeActive)
            {
                yield break;
            }
            writeActive = true;
            int lineEndIndex = Mathf.Clamp(fullText.IndexOf('\n', writeOn + 1), 0, fullText.Length); //the +1 ensures the newline is included

            if (lineEndIndex == -1)
            {
                lineEndIndex = fullText.Length;
            }

            int startWriteOn = writeOn;
            int targetWriteOn = lineEndIndex;//(lineEndIndex==fullText.Length)? fullText.Length : lineEndIndex-1;//include the newline so the writeon does not get stuck
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                int newWriteOn = (int)Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration);
                SetWriteOn(newWriteOn);
                yield return null;
            }
            writeOn = targetWriteOn;
            Refresh();
            writeActive = false;

        }
        
        /// <summary>
        /// Writes the next line over the given duration showing each character at the start of its interval.
        /// </summary>
        /// <param name="duration">The duration of the write on</param>
        /// <returns></returns>
        public IEnumerator WriteLineEarly(float duration)
        {
            if(writeActive)
            {
                yield break;
            }
            writeActive = true;
            int lineEndIndex = Mathf.Clamp(fullText.IndexOf('\n', writeOn + 1), 0, fullText.Length); //the +1 ensures the newline is included

            if (lineEndIndex == -1)
            {
                lineEndIndex = fullText.Length;
            }

            int startWriteOn = writeOn;
            int targetWriteOn = lineEndIndex;//(lineEndIndex==fullText.Length)? fullText.Length : lineEndIndex-1;//include the newline so the writeon does not get stuck
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                int newWriteOn = (int)Mathf.Ceil(Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration));
                SetWriteOn(newWriteOn);
                yield return null;
            }
            writeOn = targetWriteOn;
            Refresh();
            writeActive = false;

        }
        
        public IEnumerator PopIn(float overshoot=1.2f, float duration=0.5f)
        {
            GameObject go = this.gameObject;
            yield return AnimationManager.instance.PopIn(go, overshoot, duration);
        }

        public IEnumerator PopOut(float undershoot=0.8f, float duration=0.5f)
        {
            GameObject go = this.gameObject;
            yield return AnimationManager.instance.PopOut(go, undershoot, duration);
        }

        public IEnumerator CountDouble(double value, int decimalPlaces, float duration){
            double beginValue;
            if (!double.TryParse(GetFullText(), out beginValue)){
                beginValue = 0;
            }
            
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float prog = elapsedTime/duration;
                double mid = ((value*prog + beginValue*(1-prog))/duration);
                double rounded = Math.Round(mid, decimalPlaces);
                SetText(""+rounded);
                yield return null;
            }
            SetText(""+value);
        }

        public IEnumerator CountLong(long value, float duration){
            long beginValue;
            if (!long.TryParse(GetFullText(), out beginValue)){
                beginValue = 0;
            }
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float prog = elapsedTime/duration;
                long mid = (long)((value*prog + beginValue*(1-prog))/duration);
                SetText(""+mid);
                yield return null;
            }
            SetText(""+value);
        }

        public void SetAnimationCurve(AnimationCurve curve)
        {
            animationCurve = curve;
        }

        public void StopAnimation()
        {
            StopAllCoroutines();
        }




    }




}