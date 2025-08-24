using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class SettingUICanvas : MonoBehaviour
{
    public GameObject panel;
    public GameObject character_2;
    public List<AudioSource> audioSources;
    public Button button1;
    public Button button2;
    public MainScenePanel mainScenePanel;
    private static SettingUICanvas instance;
    public bool isInstance;
    private void Awake()
    {
        if (isInstance)
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
    }
    private void Start()
    {
        button1.onClick.AddListener(() =>
        {
            if (mainScenePanel != null)
            {
                mainScenePanel.PlayClickButton();
            }
            
        });
        button2.onClick.AddListener(() =>
        {
            if (mainScenePanel != null)
            {
                mainScenePanel.PlayClickButton();
            }
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().name != "MainScene" && SceneManager.GetActiveScene().name != "End")
            {
                OnOpenButtons();
                character_2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(277, 200), 0.5f);
                panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 0.5f);
            }
        }
    }
    public void OnCloseSetting()
    {
        character_2.GetComponent<RectTransform>().DOAnchorPos(new Vector2(277, -1600), 0.5f);
        panel.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 2000), 0.5f);
        OnCloseButtons();
    }
    public void OnCloseButtons()
    {
        // 获取父物体及其所有子物体中的所有Button组件
        Button[] allButtons = GetComponentsInChildren<Button>();

        // 遍历所有找到的Button组件
        foreach (Button btn in allButtons)
        {
            btn.interactable = false; // 设置为不可点击
            // 如果需要，也可以直接禁用整个组件
            // btn.enabled = false;
        }
    }
    public void OnOpenButtons()
    {
        // 获取父物体及其所有子物体中的所有Button组件
        Button[] allButtons = GetComponentsInChildren<Button>();

        // 遍历所有找到的Button组件
        foreach (Button btn in allButtons)
        {
            btn.interactable = true; // 设置为不可点击
            // 如果需要，也可以直接禁用整个组件
            // btn.enabled = false;
        }
    }
    public void SetAudioAdd()
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].volume += 0.2f;
            }
        }
        else
        {
            for(int i = 0;i < DialogueSystem.instance.audioObjects.Count; i++)
            {
                DialogueSystem.instance.audioObjects[i].volume += 0.2f;
                DialogueSystem.instance.defaultDialog.GetComponent<AudioSource>().volume += 0.2f;
            }
        }
    }
    public void SetAudioMinish()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].volume -= 0.2f;
            }
        }
        else
        {
            for (int i = 0; i < DialogueSystem.instance.audioObjects.Count; i++)
            {
                DialogueSystem.instance.audioObjects[i].volume -= 0.2f;
                DialogueSystem.instance.defaultDialog.GetComponent<AudioSource>().volume -= 0.2f;
            }
        }
    }
    public void BackMenu()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            SceneManager.LoadScene("MainScene");
        }
        Destroy(gameObject);
    }
}
