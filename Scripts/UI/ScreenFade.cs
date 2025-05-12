using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToNextScene()
    {
        StartCoroutine(FadeOutAndLoadNextScene());
    }

    private IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeOutAndLoadNextScene()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
}
