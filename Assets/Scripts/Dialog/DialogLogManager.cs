using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogManager : Singleton<DialogLogManager>
{
    private List<string> dialogueLog = new List<string>();

    [Header("HistoryLog UI Elements")]
    public RectTransform historyLogParent;
    public GameObject historyLogItemPrefab;  // 历史对话条预制体
    public GameObject historyLogPanel;
    public Scrollbar scrollBar;

    public void Start()
    {
        historyLogPanel.SetActive(false);
    }

    public void OnEnable()
    {
        scrollBar.value = 0f;
    }

    public void Update()
    {
        if((scrollBar.value < 0f || !scrollBar.gameObject.activeSelf) && Input.mouseScrollDelta.y < 0f)
        {
            FindObjectOfType<DefaultDialogPrefab>().Appear();
            DialogueSystem.instance.canShowNext = true;
            DialogueSystem.instance.isOpenLogPanel = false;
            gameObject.SetActive(false);
        }
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
            AddHistoryLogItem(cleanedLine);
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
    private void AddHistoryLogItem(string historyLog)
    {
        // 实例化一个新的 Item
        GameObject historyLogItem = Instantiate(historyLogItemPrefab, historyLogParent);

        // 获取 Item 内部的 TextMeshPro 组件
        TextMeshProUGUI logText = historyLogItem.GetComponent<TextMeshProUGUI>();

        // 设置 Item 中的文本
        if (logText != null)
        {
            logText.text = historyLog;
        }
    }
}
