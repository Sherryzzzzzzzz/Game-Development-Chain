using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodUI : Singleton<BloodUI>
{
    public Image bloodImage;     
    public float fadeDuration = 1f;

    public override void Awake()
    {
        base.Awake();
        bloodImage = GetComponent<Image>();
        SetAlpha(0);  // ³õÊ¼Òþ²Ø
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeToAlpha(0.5f));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = bloodImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float a)
    {
        Color c = bloodImage.color;
        c.a = a;
        bloodImage.color = c;
    }
}
