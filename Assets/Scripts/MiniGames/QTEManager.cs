using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField]public CanvasGroup promptPanel;
    [SerializeField] public Image keyIcon;
    [SerializeField] public Slider progressBar;
    [SerializeField] public Text timerText;
    [SerializeField] public Text resultText;
    [SerializeField] public Text finalResultText;
    [SerializeField] public Image backgroundOverlay;
    [Header("按钮图片")]
    [SerializeField] public Sprite keyASprite;
    [SerializeField] public Sprite keySSprite;
    [SerializeField] public Sprite keyDSprite;
    [SerializeField] public Sprite keyFSprite;
    [SerializeField] public Sprite keySpaceSprite;
    [Header("游戏设置")]
    public float reactionTime = 1.4f;
    public int totalQTE = 3;
    public float successDelay = 0.8f;
    public float failureDelay = 1.2f;
    public Color successColor = new Color(0.2f, 0.8f, 0.2f, 0.6f);
    public Color failureColor = new Color(0.8f, 0.2f, 0.2f, 0.6f);

    private KeyCode currentKey;
    private int currentQTEIndex;
    private bool isGameActive;
    private Dictionary<KeyCode, Sprite> keySpriteMap;

    public bool IsCompleted { get; private set; }
    public bool IsSuccess { get; private set; }
    private int successCount;

    private void Awake()
    {
        keySpriteMap = new Dictionary<KeyCode, Sprite>
        {
            { KeyCode.A, keyASprite },
            { KeyCode.S, keySSprite },
            { KeyCode.D, keyDSprite },
            { KeyCode.F, keyFSprite },
            { KeyCode.Space, keySpaceSprite }
        };
        ResetUI();
    }

    public void StartQTE()
    {
        if (!isGameActive)
        {
            StartCoroutine(QTEGameSequence());
        }
    }

    private IEnumerator QTEGameSequence()
    {
        isGameActive = true;
        IsCompleted = false;
        IsSuccess = false;
        successCount = 0;
        backgroundOverlay.gameObject.SetActive(true);
        backgroundOverlay.color = new Color(0, 0, 0, 0.5f);
        resultText.text = "Ready!";
        resultText.color = Color.yellow;
        resultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        resultText.gameObject.SetActive(false);

        for (int i = 0; i < totalQTE; i++)
        {
            currentQTEIndex = i + 1;
            bool qteResult = false;
            yield return StartCoroutine(RunSingleQTE(result => qteResult = result));
            if (qteResult) successCount++;
            ShowResultFeedback(qteResult);
            yield return new WaitForSeconds(qteResult ? successDelay : failureDelay);
            promptPanel.alpha = 0;
            resultText.gameObject.SetActive(false);
            backgroundOverlay.color = new Color(0, 0, 0, 0.5f);
        }

        ShowFinalResult(successCount);
        yield return new WaitForSeconds(2);
        IsCompleted = true;
        IsSuccess = successCount >= (totalQTE / 2) + 1;
        isGameActive = false;
    }

    private IEnumerator RunSingleQTE(System.Action<bool> onComplete)
    {
        bool keyPressed = false;
        bool success = false;
        float timer = reactionTime;
        currentKey = GenerateRandomKey();
        ShowKeyPrompt(currentKey);

        while (timer > 0 && !keyPressed)
        {
            timer -= Time.deltaTime;
            UpdateProgressBar(timer / reactionTime);
            if (Input.anyKeyDown)
            {
                keyPressed = true;
                success = Input.GetKeyDown(currentKey);
            }
            yield return null;
        }

        if (!keyPressed) success = false;
        onComplete?.Invoke(success);
    }

    private KeyCode GenerateRandomKey()
    {
        KeyCode[] possibleKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.Space };
        return possibleKeys[Random.Range(0, possibleKeys.Length)];
    }

    private void ResetUI()
    {
        promptPanel.alpha = 0;
        resultText.gameObject.SetActive(false);
        finalResultText.gameObject.SetActive(false);
        backgroundOverlay.gameObject.SetActive(false);
    }

    private void ShowKeyPrompt(KeyCode key)
    {
        if (keySpriteMap.ContainsKey(key))
        {
            keyIcon.sprite = keySpriteMap[key];
        }
        progressBar.value = 1f;
        timerText.text = reactionTime.ToString("F1");
        promptPanel.alpha = 1;
    }

    private void UpdateProgressBar(float progress)
    {
        progressBar.value = progress;
        timerText.text = (progress * reactionTime).ToString("F1");
        if (progress < 0.3f)
        {
            progressBar.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (progress < 0.6f)
        {
            progressBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            progressBar.fillRect.GetComponent<Image>().color = Color.green;
        }
    }

    private void ShowResultFeedback(bool success)
    {
        promptPanel.alpha = 0;
        resultText.gameObject.SetActive(true);
        if (success)
        {
            resultText.text = "Perfect";
            resultText.color = Color.green;
            backgroundOverlay.color = successColor;
        }
        else
        {
            resultText.text = "Miss";
            resultText.color = Color.red;
            backgroundOverlay.color = failureColor;
        }
        StartCoroutine(PulseText(resultText));
    }

    private void ShowFinalResult(int successCount)
    {
        finalResultText.gameObject.SetActive(true);
        bool overallSuccess = successCount >= (totalQTE / 2) + 1;
        finalResultText.text = $"{(overallSuccess ? "Finished" : "Failed")}\nSuccess: {successCount}/{totalQTE}";
        finalResultText.color = overallSuccess ? Color.green : Color.red;
        backgroundOverlay.color = overallSuccess ? successColor : failureColor;
    }

    private IEnumerator PulseText(Text textElement)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 originalScale = textElement.transform.localScale;
        while (elapsed < duration)
        {
            float scale = Mathf.Lerp(1f, 1.5f, elapsed / duration);
            textElement.transform.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0f;
        while (elapsed < duration)
        {
            float scale = Mathf.Lerp(1.5f, 1f, elapsed / duration);
            textElement.transform.localScale = originalScale * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }
        textElement.transform.localScale = originalScale;
    }
}