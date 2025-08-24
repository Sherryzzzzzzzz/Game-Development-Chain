using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class SaveManager : MonoBehaviour
{
    public SaveGameSprite saveGameSprite;
    public static SaveManager instance;
    public SaveGameCanvasUI saveGameCanvasUI;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadGame();
    }
    public void Save(int index)
    {
        DateTime now = DateTime.Now;
        string formattedDate = now.ToString("yyyy/M/d HH:mm");
        saveGameSprite.saveGameClasses[index].timetoken = formattedDate;
        saveGameSprite.saveGameClasses[index].title = SceneManager.GetActiveScene().name;
        saveGameCanvasUI.LoadNumber();
        // 将ScriptableObject转为JSON保存到文件
        string json = JsonUtility.ToJson(saveGameSprite);
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        File.WriteAllText(savePath, json);
    }
    public void Read(int index)
    {
        if (saveGameSprite.saveGameClasses[index].title != null)
        {
            SceneManager.LoadScene(saveGameSprite.saveGameClasses[index].title);
        }
    }
    public void LoadGame()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveGameSprite = ScriptableObject.CreateInstance<SaveGameSprite>();
            JsonUtility.FromJsonOverwrite(json, saveGameSprite);
        }
        else
        {
            saveGameSprite = ScriptableObject.CreateInstance<SaveGameSprite>();
            saveGameSprite.saveGameClasses = new List<SaveGameClass>
{
    new SaveGameClass(), // 索引 0
    new SaveGameClass(), // 索引 1
    new SaveGameClass(), // 索引 2
    new SaveGameClass(), // 索引 3
    new SaveGameClass(), // 索引 4
    new SaveGameClass(), // 索引 5
    new SaveGameClass(), // 索引 6
    new SaveGameClass()  // 索引 7
};
            Debug.Log("没有存档");
        }
    }
}
