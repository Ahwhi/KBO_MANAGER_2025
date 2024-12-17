using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float logoDisplayTime = 3f;
    public GameObject LogoCanvas;
    public Image BackGround;
    public Image Logo;
    private CanvasGroup logoCanvasGroup;

    void Start()
    {
        if (!GameDirector.isLogo)
        {
            GameDirector.isLogo = true;
            LogoCanvas.SetActive(true);
            logoCanvasGroup = Logo.GetComponent<CanvasGroup>();
            if (logoCanvasGroup == null)
            {
                logoCanvasGroup = Logo.gameObject.AddComponent<CanvasGroup>();
            }
            logoCanvasGroup.alpha = 0f;
            StartCoroutine(DisplayLogo());
        }
    }

    IEnumerator DisplayLogo()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(logoDisplayTime);
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            logoCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        logoCanvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            logoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        logoCanvasGroup.alpha = 0f;
        LogoCanvas.SetActive(false);
    }
}
