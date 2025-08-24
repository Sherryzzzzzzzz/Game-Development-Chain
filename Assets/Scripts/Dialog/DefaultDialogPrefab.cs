using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefaultDialogPrefab : MonoBehaviour
{
    public Image dialogBox;
    public Text dialogText;
    private CanvasGroup canvasGroup;
    //private Color dialogBoxColor;
    //private Color dialogTextColor;
    [Header("历史记录")]
    public Button historyButton;
    [Header("存档")]
    public Button saveGameButton;
    [Header("快进")]
    public Button fastForwardButton;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if (scene.name == "MainScene"|| scene.name == "End")
        {
            Disappear();
            DialogueSystem.instance.canShowNext = false;
            DialogueSystem.instance.isFastForward = false;
            DialogueSystem.instance.ClearGameObjects();
        }
        else
        {
            DialogueSystem.instance.canShowNext = true;
            if(scene.name == "01Dream")
            {
                DialogueSystem.fs.ReadTextFromResource("start");
                Appear();
            }
            else if(scene.name == "02Livebroadcast")
            {
                DialogueSystem.fs.RemoveAllText();
            }
            else
            {
                DialogueSystem.fs.RemoveAllText();
            }
        }
    }
    public void Start()
    {
        DialogueSystem.instance.defaultDialog = this;
        fastForwardButton.onClick.AddListener(()=> 
        {
            GetComponent<AudioSource>().Play();
            DialogueSystem.instance.isFastForward = !DialogueSystem.instance.isFastForward;
        });
        historyButton.onClick.AddListener(() =>
        {
            GetComponent<AudioSource>().Play();
            DialogueSystem.instance.OnClickHistoryButton();
        });
        //dialogBoxColor = dialogBox.color;
        //dialogTextColor = dialogText.color;
        saveGameButton.onClick.AddListener(() =>
        {
            GetComponent<AudioSource>().Play();
            GameObject.Instantiate(Resources.Load<GameObject>("SaveGameCanvas"));
        });
        DialogueSystem.instance.canShowNext = false;
        Disappear();
    }
    public void Disappear()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;  // 取消遮挡，允许点击穿透
        canvasGroup.interactable = false;    // 禁用交互（可选）
        //dialogBox.color = Color.clear;
        //dialogText.color = Color.clear;
    }

    public void Appear()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;  // 取消遮挡，允许点击穿透
        canvasGroup.interactable = true;    // 禁用交互（可选）
        //dialogBox.color = dialogBoxColor;
        //dialogText.color = dialogTextColor;
    }
}
