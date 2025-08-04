using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : Singleton<DialogueSystem>
{
    public static FlowerSystem fs;
    public bool canShowNext;

    public bool isOpenLogPanel;

    void Start()
    {
        canShowNext = true;

        //DontDestroyOnLoad(this.gameObject);
        fs = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
        fs.SetupDialog();
        fs.ReadTextFromResource("start");
        fs.RegisterCommand("ChangeScene", (List<string> _params) =>
        {
            SceneManager.LoadScene(_params[0]);
        });
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0f && !isOpenLogPanel)
        {
            canShowNext = false;
            FindObjectOfType<DefaultDialogPrefab>().Disappear();
            DialogLogManager.instance.gameObject.SetActive(true);
            //DialogLogManager.instance.scrollBar.value = 0f;
            isOpenLogPanel = true;
        }

        if (Input.anyKeyDown && canShowNext)
        {
            fs.Next();
        }
    }



    public static void StartDialogue(string path)
    {
        
        fs.ReadTextFromResource(path);
    }
    
}
