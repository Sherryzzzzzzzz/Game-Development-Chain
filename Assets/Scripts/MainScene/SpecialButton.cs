using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


public class SpecialButton : MonoBehaviour, IPointerExitHandler,IPointerEnterHandler
{
    public MainScenePanel mainScenePanel;
    
    Vector3 targetPosition;

    private int count = 0;
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (count <= 10)
        {
            targetPosition.x = Random.Range(mainScenePanel.mainPanelRect.xMin, mainScenePanel.mainPanelRect.xMax);
            targetPosition.y = Random.Range(mainScenePanel.mainPanelRect.yMin, mainScenePanel.mainPanelRect.yMax);
            targetPosition = mainScenePanel.transform.TransformPoint(targetPosition);
            transform.position = targetPosition;
            mainScenePanel.PlayAudio(mainScenePanel.appearImage);
            count++;
        }
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnClick()
    {
        mainScenePanel.settingButton.interactable = false;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        Image buttonImage = GetComponentInChildren<Image>();
        while (buttonImage.color.a > 0)
        {
            var color = buttonImage.color;
            color.a = color.a - 0.1f;
            buttonImage.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        mainScenePanel.ShowSpecial();
        mainScenePanel.PlayAudio(mainScenePanel.ciallo);
    }
}
