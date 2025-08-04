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
    public GameObject historyLogItemPrefab;  // ��ʷ�Ի���Ԥ����
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

        // �ж�����line�Ƿ���һ�������Ű��������ݣ���ͷ��[����β��]��
        // ^\[[^\]]*\]$ ��ʾ�����ַ����� [ ... ] ��ʽ
        if (Regex.IsMatch(line, @"^\[[^\]]*\]$"))
        {
            // ���������������ݣ�ֱ�ӷ��أ�����¼
            return;
        }

        // ����ɾ�����������ż��������ݣ����� [lr] [w] ��ɾ��
        string cleanedLine = Regex.Replace(line, @"\[[^\]]*\]", "");

        // ȥ��������ַ���ǰ�����հ�
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

    // ��̬�����ʷ��¼��
    private void AddHistoryLogItem(string historyLog)
    {
        // ʵ����һ���µ� Item
        GameObject historyLogItem = Instantiate(historyLogItemPrefab, historyLogParent);

        // ��ȡ Item �ڲ��� TextMeshPro ���
        TextMeshProUGUI logText = historyLogItem.GetComponent<TextMeshProUGUI>();

        // ���� Item �е��ı�
        if (logText != null)
        {
            logText.text = historyLog;
        }
    }
}
