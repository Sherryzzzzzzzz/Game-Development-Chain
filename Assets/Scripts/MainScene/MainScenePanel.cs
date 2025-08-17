using System;
using System.Collections;
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
    [SerializeField] public Button settingButton;
    [SerializeField] public Button exitButton;
    [SerializeField] public AudioClip clickButton;
    [SerializeField] public AudioClip appearImage;
    [SerializeField] public AudioClip ciallo;
    
    private GameBegin gameBegin;
    private AudioSource audioSource;
    [NonSerialized] public Rect mainPanelRect;
    public Image repeatImage;

    [Space(10)] 
    public float moveOutTime;
    
    [Space(10)]
    public float intervalTime;
    public int repeatTimes;


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
            startButton.onClick.RemoveListener(gameBegin.startGame);
            startButton.onClick.AddListener(gameBegin.startGame);
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

        
    }

    private void PlayClickButton()
    {
        PlayAudio(clickButton);
    }

    public void PlayAudio(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void ShowSpecial()
    {
        character_2.transform.DOMoveY(-character_2.transform.position.y, 0.5f);
    }
}
