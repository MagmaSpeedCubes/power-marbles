using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using MagmaLabs.Animation;
using MagmaLabs.Utilities.Editor;
namespace MagmaLabs.UI{
    [CreateAssetMenu(fileName = "TMPEnhanced", menuName = "MagmaLabs/UI/TMPEnhanced", order = 1)]

public class TMPEnhanced : TextMeshProUGUI
    {
        private static readonly int DEBUG_INFO_LEVEL = 2;
        public string fullText = "";
        public int writeOn = 0;
        private bool writeActive = false;

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

        public void AddText(string text)
        {
            fullText += text;
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
            return writeOn / fullText.Length;
        }
        /// <summary>
        /// Writes the next line over the given duration showing each character at the end of its interval.
        /// </summary>
        /// <param name="targetWriteOn"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public IEnumerator WriteOn(int targetWriteOn, float duration)
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


        public IEnumerator WriteLine(float duration)
        {
            if(writeActive)
            {
                DebugEnhanced.LogInfoLevel("WriteLine called but writeActive is true, exiting coroutine", 2, DEBUG_INFO_LEVEL);
                yield break;
            }
            writeActive = true;
            DebugEnhanced.LogInfoLevel("Starting WriteLine coroutine", 2, DEBUG_INFO_LEVEL);
            int lineEndIndex = Mathf.Clamp(fullText.IndexOf('\n', writeOn + 1), 0, fullText.Length); //the +1 ensures the newline is included
            DebugEnhanced.LogInfoLevel("Line end index: " + lineEndIndex, 2, DEBUG_INFO_LEVEL);

            if (lineEndIndex == -1)
            {
                lineEndIndex = fullText.Length;
                DebugEnhanced.LogInfoLevel("No newline found, setting line end index to full text length: " + lineEndIndex, 2, DEBUG_INFO_LEVEL);
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
            while (writeOn < fullText.Length - 1 && fullText[writeOn + 1] == '\n')
            {
                DebugEnhanced.LogInfoLevel("New line character detected at " + writeOn, 2, DEBUG_INFO_LEVEL);
                writeOn += (int)(1f / fullText.Length);
            }
            DebugEnhanced.LogInfoLevel("Final writeOn set to target: " + writeOn, 2, DEBUG_INFO_LEVEL);
            Refresh();

            DebugEnhanced.LogInfoLevel("WriteLine coroutine complete", 2, DEBUG_INFO_LEVEL);
            writeActive = false;

        }
        
        /// <summary>
        /// Writes the next line over the given duration showing each character at the start of its interval.
        /// </summary>
        /// <param name="duration">The duration of the write on</param>
        /// 
        public IEnumerator WriteLineEarly(float duration)
        {
            if(writeActive)
            {
                DebugEnhanced.LogInfoLevel("WriteLine called but writeActive is true, exiting coroutine", 2, DEBUG_INFO_LEVEL);
                yield break;
            }
            writeActive = true;
            DebugEnhanced.LogInfoLevel("Starting WriteLine coroutine", 2, DEBUG_INFO_LEVEL);
            int lineEndIndex = Mathf.Clamp(fullText.IndexOf('\n', writeOn + 1), 0, fullText.Length); //the +1 ensures the newline is included
            DebugEnhanced.LogInfoLevel("Line end index: " + lineEndIndex, 2, DEBUG_INFO_LEVEL);

            if (lineEndIndex == -1)
            {
                lineEndIndex = fullText.Length;
                DebugEnhanced.LogInfoLevel("No newline found, setting line end index to full text length: " + lineEndIndex, 2, DEBUG_INFO_LEVEL);
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
            while (writeOn < fullText.Length - 1 && fullText[writeOn + 1] == '\n')
            {
                DebugEnhanced.LogInfoLevel("New line character detected at " + writeOn, 2, DEBUG_INFO_LEVEL);
                writeOn += (int)(1f / fullText.Length);
            }
            DebugEnhanced.LogInfoLevel("Final writeOn set to target: " + writeOn, 2, DEBUG_INFO_LEVEL);
            Refresh();

            DebugEnhanced.LogInfoLevel("WriteLine coroutine complete", 2, DEBUG_INFO_LEVEL);
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


    }


}