using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickToDialog : MonoBehaviour
{
    public UnityEvent OnClick = new UnityEvent();
    public string dialogTxtName;

    private bool isClicked;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (Input.GetMouseButtonDown(0) && !isClicked)
        {
            if (hit != null && hit.transform == transform)
            {
                if (FindObjectOfType<DialogueSystem>())
                {
                    Debug.Log("进行一个击的点");
                    DialogueSystem.StartDialogue(dialogTxtName);
                }

                OnClick.Invoke();
                isClicked = true;
                
            }
        }
    }
}
