using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using MagmaLabs.Animation;
namespace MagmaLabs.UI{
    public class Alert : MonoBehaviour
    {
        public CanvasGroup entire;
        public Image background;
        public Image icon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI body;
        public float duration;

        void Start()
        {
            StartCoroutine(LifetimeCoroutine());
        }

        IEnumerator LifetimeCoroutine()
        {
            yield return StartCoroutine(Animations.instance.PopIn(gameObject, 1.1f, duration * 0.1f));
            yield return new WaitForSeconds(duration * 0.7f);
            yield return StartCoroutine(Animations.instance.FadeUI(gameObject, 1f, 0f, duration * 0.2f));
            Destroy(gameObject);
        }
    }
}
