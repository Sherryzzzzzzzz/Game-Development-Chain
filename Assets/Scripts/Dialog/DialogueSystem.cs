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
                fs.Next();
                currentIntervalTimes = intervalTimes;
            }
        }
    }
    public static void StartDialogue(string path)
    {
        Debug.Log(path);
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
