using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultDialogPrefab : MonoBehaviour
{
    public Image dialogBox;
    public Text dialogText;

    private Color dialogBoxColor;
    private Color dialogTextColor;

    public void Start()
    {
        dialogBoxColor = dialogBox.color;
        dialogTextColor = dialogText.color;
    }

    public void Disappear()
    {
        dialogBox.color = Color.clear;
        dialogText.color = Color.clear;
    }

    public void Appear()
    {
        dialogBox.color = dialogBoxColor;
        dialogText.color = dialogTextColor;
    }
}
