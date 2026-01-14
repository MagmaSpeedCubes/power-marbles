using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace MagmaLabs.UI{
public class TextMeshMaxUGUI : TextMeshProUGUI
    {
        private string fullText;
        private float writeOn = 1f;
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
            base.text = text;
        }

        public string GetText()
        {
            return base.text;
        }

        public string GetFullText()
        {
            return fullText;
        }

        public void SetWriteOn(float nw)
        {
            writeOn = nw;
            float percentage = text.Length / writeOn;
            int characterCutoff = (int) (percentage * text.Length);
            base.text = fullText.Substring(0, characterCutoff);
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




    }

}