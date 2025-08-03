using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ClickWorld : MonoBehaviour
{
    public UnityEvent OnClickWorld = new UnityEvent();
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (Input.GetMouseButtonDown(0))
        {
            if (hit != null && hit.transform == transform)
            {
                DialogueSystem.StartDialogue("Live");
                OnClickWorld.Invoke();
            }
        }
    }
}
