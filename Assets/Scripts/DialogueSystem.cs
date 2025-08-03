using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using UnityEngine.SceneManagement;

public class DialogueSystem : MonoBehaviour
{
    public static FlowerSystem fs;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        fs = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
        fs.SetupDialog();
        fs.ReadTextFromResource("start");
        fs.RegisterCommand("ChangeScene", (List<string> _params) =>
        {
            SceneManager.LoadScene(_params[0]);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            fs.Next();
        }
    }

    public static void StartDialogue(string path)
    {
        
        fs.ReadTextFromResource(path);
    }
    
}
