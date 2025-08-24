using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArchiveCanvasSet : MonoBehaviour
{
    public Button yes;
    public Button no;
    public Button close;
    public GameObject clickedObject;
    public Text saveOrLoadText;
    private bool isSave;
    private void Awake()
    {
        no.onClick.AddListener(OnClickNO);
        close.onClick.AddListener(OnClickNO);
        yes.onClick.AddListener(OnClickYes);
    }
    public void OnEnableSet(bool isSave,GameObject click)
    {
        clickedObject = click;
        this.isSave = isSave;
        if (isSave)
        {
            saveOrLoadText.text = "ÊÇ·ñ´æµµ";
        }
        else
        {
            saveOrLoadText.text = "ÊÇ·ñ¶Áµµ";
        }
        if(SceneManager.GetActiveScene().name != "MainScene" && !isSave)
        {
            saveOrLoadText.text = "ÊÇ·ñ¸²¸Ç";
        }
    }
    public void OnClickYes()
    {
        if (isSave)
        {
            if(SceneManager.GetActiveScene().name !="MainScene") clickedObject.GetComponent<NumberButton>().SetSave();
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                clickedObject.GetComponent<NumberButton>().SetRead();
            }
            else
            {
                clickedObject.GetComponent<NumberButton>().SetSave();
            }
        }
        Destroy(gameObject);
    }
    public void OnClickNO()
    {
        Destroy(gameObject);
    }
}
