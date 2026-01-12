using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SplashScreenManager : MonoBehaviour
{
    [Header("Splash Audio")]
    [Tooltip("Clip to play when splash screen starts (first launch only if enabled).")]
    [SerializeField] private AudioClip splashClip;
    [SerializeField, Range(0f, 1f)] private float splashVolume = 1f;
    [Tooltip("If true, play only the very first time the game is launched.")]
    [SerializeField] private bool onlyOnFirstLaunch = true;

    private const string PlayerPrefKey = "HasSeenSplash_v1";

    IEnumerator Start()
    {
        Debug.Log("Showing splash screen");

        // Decide whether to play
        bool firstRun = PlayerPrefs.GetInt(PlayerPrefKey, 0) == 0;
        if (splashClip != null && (!onlyOnFirstLaunch || firstRun))
        {
            // Ensure an AudioSource exists and is 2D (non-spatial)
            AudioSource src = GetComponent<AudioSource>();
            if (src == null)
            {
                src = gameObject.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.loop = false;
                src.spatialBlend = 0f; // 0 = 2D sound
            }

            src.PlayOneShot(splashClip, splashVolume);

            if (onlyOnFirstLaunch && firstRun)
            {
                PlayerPrefs.SetInt(PlayerPrefKey, 1);
                PlayerPrefs.Save();
            }
        }

        SplashScreen.Begin();
        while (!SplashScreen.isFinished)
        {
            SplashScreen.Draw();
            yield return null;
        }

        Debug.Log("Finished showing splash screen");
    }
}