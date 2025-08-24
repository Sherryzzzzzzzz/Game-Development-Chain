using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogManager : Singleton<DialogLogManager>
{
    private List<string> dialogueLog = new List<string>();

    private List<TextMeshProUGUI> dialogueTextMeshProUGUIs = new List<TextMeshProUGUI>();

    [Header("HistoryLog UI Elements")]
    public RectTransform historyLogParent;
    public GameObject historyLogItemPrefab;  // 历史对话条预制
    public GameObject historyLogPanel;
    public Scrollbar scrollBar;
    public Button button;

    private int head;
    private int tail;

    public void Start()
    {
        button.onClick.AddListener(() => OnCloseHistoryInspector());
        historyLogPanel.SetActive(false);
    }

    public void OnEnable()
    {
        scrollBar.value = 0.01f;
    }

    public void Update()
    {
        if(((scrollBar.value < 0.01f || !scrollBar.gameObject.activeSelf) && Input.mouseScrollDelta.y < 0f) || Input.GetMouseButtonDown(1))
        {
            OnCloseHistoryInspector();
        }
    }
    private void OnCloseHistoryInspector()
    {
        FindObjectOfType<DefaultDialogPrefab>().Appear();
        DialogueSystem.instance.canShowNext = true;
        DialogueSystem.instance.isOpenLogPanel = false;
        gameObject.SetActive(false);
    }

    public void LogLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return;

        // 判断整条line是否是一个中括号包裹的内容（开头是[，结尾是]）
        // ^\[[^\]]*\]$ 表示整条字符串是 [ ... ] 格式
        if (Regex.IsMatch(line, @"^\[[^\]]*\]$"))
        {
            // 整条是中括号内容，直接返回，不记录
            return;
        }

        // 否则，删除所有中括号及其中内容，比如 [lr] [w] 都删除
        string cleanedLine = Regex.Replace(line, @"\[[^\]]*\]", "");

        // 去除清理后字符串前后多余空白
        cleanedLine = cleanedLine.Trim();

        if (!string.IsNullOrEmpty(cleanedLine))
        {
            dialogueLog.Add(cleanedLine);
            AddHistoryLogItem();
        }
    }

    public List<string> GetFullLog()
    {
        return new List<string>(dialogueLog);
    }

    public void ClearLog()
    {
        dialogueLog.Clear();
    }

    // 动态添加历史记录条
    private void AddHistoryLogItem()
    {
        if (dialogueLog.Count <= 49)
        {
            // 实例化一个新的 Item
            GameObject historyLogItem = Instantiate(historyLogItemPrefab, historyLogParent);
            // 获取 Item 内部的 TextMeshPro 组件
            TextMeshProUGUI logText = historyLogItem.GetComponent<TextMeshProUGUI>();
            dialogueTextMeshProUGUIs.Add(logText);
            // 设置 Item 中的文本
            if (logText != null)
            {
                logText.text = dialogueLog[dialogueLog.Count - 1];
            }
            tail = dialogueLog.Count - 1;
        }
        else
        {
            head = dialogueLog.Count - 50;
            TextMeshProUGUI logText = dialogueTextMeshProUGUIs[0];
            dialogueTextMeshProUGUIs.RemoveAt(0);
            if (logText != null)
            {
                logText.text = dialogueLog[dialogueLog.Count - 1]; ;
            }
            logText.transform.SetAsLastSibling();
            dialogueTextMeshProUGUIs.Add(logText);
        }
    }
    public bool UpdateForUp()
    {
        // 检查是否有足够的对话记录可以上翻
        if (dialogueLog.Count <= dialogueTextMeshProUGUIs.Count)
            return false; // 对话数量少于显示框数量，无法上翻

        // 计算新的起始索引（向上移动一个页面）
        int newStartIndex = head - dialogueTextMeshProUGUIs.Count;

        // 检查是否已经到达顶部
        if (newStartIndex < 0)
        {
            // 已经到达最顶部，显示最开始的内容
            newStartIndex = 0;
            head = newStartIndex;
            tail = newStartIndex + dialogueTextMeshProUGUIs.Count - 1;

            // 更新显示内容
            for (int i = 0; i < dialogueTextMeshProUGUIs.Count; i++)
            {
                int logIndex = newStartIndex + i;
                if (logIndex < dialogueLog.Count)
                {
                    dialogueTextMeshProUGUIs[i].text = dialogueLog[logIndex];
                }
            }

            return false; // 已到达顶部
        }
        else
        {
            // 可以正常上翻
            head = newStartIndex;
            tail = newStartIndex + dialogueTextMeshProUGUIs.Count - 1;

            // 更新显示内容
            for (int i = 0; i < dialogueTextMeshProUGUIs.Count; i++)
            {
                int logIndex = newStartIndex + i;
                dialogueTextMeshProUGUIs[i].text = dialogueLog[logIndex];
            }

            return true; // 成功上翻
        }
    }
    public bool UpdateForDown()
    {
        // 检查是否有足够的对话记录可以下翻
        if (dialogueLog.Count <= dialogueTextMeshProUGUIs.Count)
            return false;

        // 如果已经在最底部，无法下翻
        if (tail >= dialogueLog.Count - 1)
            return false;

        // 计算新的起始索引（当前tail的下一个位置开始）
        int newStartIndex = head + dialogueTextMeshProUGUIs.Count;

        // 确保不超过日志底部
        if (newStartIndex + dialogueTextMeshProUGUIs.Count > dialogueLog.Count)
        {
            newStartIndex = dialogueLog.Count - dialogueTextMeshProUGUIs.Count;
        }

        // 更新head和tail
        head = newStartIndex;
        tail = Mathf.Min(newStartIndex + dialogueTextMeshProUGUIs.Count - 1, dialogueLog.Count - 1);

        // 更新显示内容
        for (int i = 0; i < dialogueTextMeshProUGUIs.Count; i++)
        {
            int logIndex = head + i;
            if (logIndex < dialogueLog.Count)
            {
                dialogueTextMeshProUGUIs[i].text = dialogueLog[logIndex];
            }
            else
            {
                dialogueTextMeshProUGUIs[i].text = ""; // 清空超出范围的文本框
            }
        }

        // 检查是否到达底部
        return tail < dialogueLog.Count - 1;
    }
}
