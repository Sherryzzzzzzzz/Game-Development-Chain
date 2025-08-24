using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SaveGameCanvasUI : MonoBehaviour
{
    private SaveGameSprite saveGameSprite => SaveManager.instance.saveGameSprite;
    public GameObject[] gameObjects;
    private void OnEnable()
    {
        SaveManager.instance.saveGameCanvasUI = this;
        if (DialogueSystem.instance)
        {
            DialogueSystem.instance.canShowNext = false;
        }
        
        LoadNumber();
    }
    private void OnDisable()
    {
        if (DialogueSystem.instance)
        {
            DialogueSystem.instance.canShowNext = true;
        }
    }
    public void LoadNumber()
    {
        for(int i = 0; i < gameObjects.Length&&i< saveGameSprite.saveGameClasses.Count; i++)
        {
            if (saveGameSprite.saveGameClasses[i].timetoken != "")
            {
                gameObjects[i].transform.GetChild(0).gameObject.SetActive(true);
                gameObjects[i].transform.GetChild(1).gameObject.SetActive(true);
                gameObjects[i].GetComponent<NumberButton>().isSaved = false;
                gameObjects[i].transform.GetChild(1).GetComponent<Text>().text = saveGameSprite.saveGameClasses[i].timetoken;
            }
            else
            {
                gameObjects[i].GetComponent<NumberButton>().isSaved = true;
                gameObjects[i].transform.GetChild(0).gameObject.SetActive(false);
                gameObjects[i].transform.GetChild(1).gameObject.SetActive(false);
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
    public void DestroyGameObject()
    {
        
        Destroy(gameObject);
    }
}
