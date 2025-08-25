using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueSystem : Singleton<DialogueSystem>
{
    public GameObject historyObject;

    public static FlowerSystem fs;
    public bool canShowNext;
    public bool isOpenLogPanel;
    public List<GameObject> gameObjects;//在回到主页面时要删除的物体
    public List<AudioSource> audioObjects;
    public bool isFastForward;
    public float intervalTimes = 0.02f;
    private float currentIntervalTimes;
    [Header("小游戏配置")]
    public GameObject QTEPrefab;
    public FlowerSystemExtension FSysExtension;

    public DefaultDialogPrefab defaultDialog;
    
    // 添加一个变量来跟踪上一次的等待状态
    private bool lastWaitingForNext = false;
    // 添加计时器和标志位
    private float lastTextUpdateTime = 0f;
    private bool shouldShowTag = false;

    private void Start()
    {
        currentIntervalTimes = intervalTimes;
        canShowNext = true;

        //DontDestroyOnLoad(this.gameObject);
        fs = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
        fs.SetupDialog();
        FSysExtension.flowerSystem = fs;
        MiniGameManager.instance.flowerSystem = fs;

        fs.RegisterCommand("ChangeScene", (List<string> _params) =>
        {
            SceneManager.LoadScene(_params[0]);
        });
        fs.RegisterCommand("ShowBloodUI", (List<string> _params) => {
            BloodUI.instance?.StartFadeIn();
        });
        fs.RegisterCommand("CloseBloodUI", (List<string> _params) => {
            BloodUI.instance?.StartFadeOut();
        });
        
        // 注册文本播放完成事件，播放完成后显示tag
        // 修改事件注册方式，使用textUpdated事件来监听文本更新
        fs.textUpdated += (sender, args) => {
            // 更新最后文本更新时间
            lastTextUpdateTime = Time.time;
            shouldShowTag = true;
            
            // 只有当状态从非等待变为等待时，才准备显示tag
            // 这意味着文本显示完成，正在等待用户输入
            if (fs.isWaitingForNext && !lastWaitingForNext) {
                // 不再立即显示tag，而是设置标志位
                // defaultDialog?.ShowTag();
            } 
            // 只有当状态从等待变为非等待时，才隐藏tag
            // 这意味着用户点击了继续，开始新的文本显示
            else if (!fs.isWaitingForNext && lastWaitingForNext) {
                defaultDialog?.HideTag();
                shouldShowTag = false;
            }
            
            // 更新lastWaitingForNext状态
            lastWaitingForNext = fs.isWaitingForNext;
        };
    }
    public void OnClickHistoryButton()
    {
        canShowNext = false;
        FindObjectOfType<DefaultDialogPrefab>().Disappear();
        DialogLogManager.instance?.gameObject.SetActive(true);
        //DialogLogManager.instance.scrollBar.value = 0f;
        isOpenLogPanel = true;
    }

    void Update()
    {
        // 检查是否需要显示tag（0.5秒内没有文本更新）
        if (shouldShowTag && Time.time - lastTextUpdateTime >= 0.5f)
        {
            defaultDialog?.ShowTag();
            shouldShowTag = false;
        }
        
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "End") return;
        // 检查当前选中的对象是否是Button
        bool isClickingButton = EventSystem.current.currentSelectedGameObject != null &&
                               EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null;
        if (Input.mouseScrollDelta.y > 0f && !isOpenLogPanel)
        {
            canShowNext = false;
            FindObjectOfType<DefaultDialogPrefab>().Disappear();
            DialogLogManager.instance?.gameObject.SetActive(true);
            //DialogLogManager.instance.scrollBar.value = 0f;
            isOpenLogPanel = true;
        }
        if ((!historyObject.activeSelf&&Input.anyKeyDown && !isClickingButton && !Input.GetKeyDown(KeyCode.Escape)) || (Input.mouseScrollDelta.y < 0f && canShowNext))
        {
            isFastForward = false;
            // 在继续对话前隐藏tag
            defaultDialog?.HideTag();
            // 重置状态跟踪变量
            lastWaitingForNext = false;
            shouldShowTag = false;
            fs.Next();
        }
        if (isFastForward&& !historyObject.activeSelf)
        {
            OnNext();
        }
    }
    void OnNext()
    {
        currentIntervalTimes -= Time.deltaTime;
        if (currentIntervalTimes < 0)
        {
            if (canShowNext)
            {
                // 在继续对话前隐藏tag
                defaultDialog?.HideTag();
                // 重置状态跟踪变量
                lastWaitingForNext = false;
                shouldShowTag = false;
                fs.Next();
                currentIntervalTimes = intervalTimes;
            }
        }
    }
    public static void StartDialogue(string path)
    {
        Debug.Log(path);
        // 开始新对话时隐藏tag
        instance.defaultDialog?.HideTag();
        // 重置状态跟踪变量
        instance.lastWaitingForNext = false;
        instance.shouldShowTag = false;
        fs.ReadTextFromResource(path);
    }
    public void ClearGameObjects()
    {
        if (gameObjects == null) return;
        if(gameObjects.Count > 0)
        {
            defaultDialog.dialogText.text = "";
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if(gameObjects[i] != null)
                {
                    Destroy(gameObjects[i]);
                }
            }
        }
    }
}