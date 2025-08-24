using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainScenePanel : MonoBehaviour
{
    [SerializeField] private Image title_931;
    [SerializeField] private Image title_Dreams;
    [SerializeField] private Image character;
    [SerializeField] private Image character_2;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] public Button startButton;
    [SerializeField] public Button loadButton;
    [SerializeField] public Button settingButton;
    [SerializeField] public Button exitButton;
    [SerializeField] public AudioClip clickButton;
    [SerializeField] public AudioClip appearImage;
    [SerializeField] public AudioClip ciallo;
    [Header("设置")]
    public GameObject settingObject;
    private GameBegin gameBegin;
    private AudioSource audioSource;
    [NonSerialized] public Rect mainPanelRect;
    public Image repeatImage;

    [Space(10)] 
    public float moveOutTime;
    
    [Space(10)]
    public float intervalTime;
    public int repeatTimes;

    public List<AudioSource> audioSources;
    void Awake()
    {
        
        mainPanelRect = gameObject.GetComponent<RectTransform>().rect;
        audioSource = GetComponent<AudioSource>();
        
        InitButton();
        
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }

    private void InitButton()
    {
        UnityAction clickButtonAction = PlayClickButton;
        startButton.onClick.AddListener(clickButtonAction);
        gameBegin = gameBegin == null ? new GameBegin(this) : gameBegin;
        if (gameBegin != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(gameBegin.startGame);
            startButton.onClick.AddListener(() => FadeOut());
        }
        else
        {
            Debug.LogError("GameBegin is null");
        }
        SpecialButton specialSetting = settingButton.gameObject.AddComponent<SpecialButton>();
        specialSetting.mainScenePanel = this;
        settingButton.onClick.AddListener(clickButtonAction);
        settingButton.onClick.AddListener(specialSetting.OnClick);
        exitButton.onClick.AddListener(clickButtonAction);
        exitButton.onClick.AddListener(Application.Quit);
        loadButton.onClick.AddListener(clickButtonAction);
        loadButton.onClick.AddListener(() => GameObject.Instantiate(Resources.Load<GameObject>("SaveGameCanvas")));
        
    }

    public void PlayClickButton()
    {
        PlayAudio(clickButton);
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void ShowSpecial()
    {
        character_2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(277, 200), 0.5f);
        settingObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,0),0.5f);
        settingObject.GetComponent<SettingUICanvas>().OnOpenButtons();
    }
    // 调用此方法开始淡出
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        for(int i = 0; i < audioSources.Count; i++)
        {
            float startVolume = audioSources[i].volume;

            // 逐渐降低音量
            while (audioSources[i].volume > 0)
            {
                audioSources[i].volume -= startVolume * Time.deltaTime / 2f;
                yield return null;
            }

            // 完全停止音频
            audioSource.Stop();

            // 可选：重置音量（如果后续还要播放）
            audioSource.volume = startVolume;
        }

    }
}
