using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSetAboutImage : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image;
    public float keepTimes;
    private int index;
    private void Awake()
    {
        image.sprite = sprites[0];
    }
    private void Start()
    {
        StartCoroutine(FadeCycle());
    }
    IEnumerator FadeCycle()
    {
        // 无限循环
        while (true)
        {
            // 第一步：从1淡出到0
            yield return StartCoroutine(FadeImage(1f, 0f, 1f));
            // 切换图片
            SwitchToNextImage();

            // 第二步：从0淡入到1
            yield return StartCoroutine(FadeImage(0f, 1f, 1f));
            // 等待指定时间
            yield return new WaitForSeconds(keepTimes);
        }
    }

    IEnumerator FadeImage(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            color.a = newAlpha;
            image.color = color;
            yield return null;
        }

        // 确保最终颜色正确
        color.a = targetAlpha;
        image.color = color;
    }

    void SwitchToNextImage()
    {
        index++;
        if(index >= sprites.Count)
        {
            index = 0;
        }
        image.sprite = sprites[index];
    }
}
