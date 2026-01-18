using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using MagmaLabs.Animation;
namespace MagmaLabs.UI{
    [CreateAssetMenu(fileName = "TMPEnhanced", menuName = "MagmaLabs/UI/TMPEnhanced", order = 1)]

public class TMPEnhanced : TextMeshProUGUI
    {
        public bool m_outlineText;
        private string fullText = "";
        private float writeOn = 1f;

        private void Refresh()
        {
            int characterCutoff = (int) (writeOn * fullText.Length);
            base.text = fullText.Substring(0, characterCutoff);
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
            Refresh();
        }

        public void AddText(string text)
        {
            fullText += text;
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
            writeOn = nw;
            Refresh();
        }

        public float GetWriteOn()
        {
            return writeOn;
        }

        public IEnumerator WriteOn(float targetWriteOn, float duration)
        {
            float startWriteOn = writeOn;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newWriteOn = Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration);
                SetWriteOn(newWriteOn);
                yield return null;
            }
        }

        public IEnumerator WriteLine(float duration)
        {
            int lineEndIndex = fullText.IndexOf('\n');
            if (lineEndIndex == -1)
            {
                lineEndIndex = fullText.Length;
            }

            float startWriteOn = writeOn;
            float targetWriteOn = (float)lineEndIndex / (float)fullText.Length + 1;//include the newline so the writeon does not get stuck
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newWriteOn = Mathf.Lerp(startWriteOn, targetWriteOn, elapsedTime / duration);
                SetWriteOn(newWriteOn);
                yield return null;
            }
            writeOn = targetWriteOn;
            Debug.Log("WriteOn complete: " + writeOn);
            Debug.Log("Full text length: " + fullText.Length);
            Debug.Log("Displayed text length: " + base.text.Length);

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