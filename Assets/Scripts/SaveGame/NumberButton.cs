using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class NumberButton : MonoBehaviour
{
    Button button;
    public bool isSaved;
    [Header("子物体")]
    public int index;
    public Text timeToken;
    public string numbers;
    public GameObject[] childrens;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnButtoClick);
    }
    private void OnButtoClick()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("ArchiveCanvas"));
        go.GetComponent<ArchiveCanvasSet>().OnEnableSet(isSaved,gameObject);
    }
    public void SetSave()
    {
        SaveManager.instance.Save(index);
        //保存到配置表再赋值
    }
    public void SetRead()
    {
        DialogueSystem.instance.ClearGameObjects();
        SaveManager.instance.Read(index);
    }
}
