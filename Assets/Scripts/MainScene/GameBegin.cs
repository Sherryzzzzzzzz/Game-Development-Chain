using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameBegin
{
    public UnityAction startGame;
    
    private CoroutineRunner coroutineRunner;
    private MainScenePanel mainScenePanel;

    public GameBegin(MainScenePanel mainScenePanel)
    {
        this.mainScenePanel = mainScenePanel;
        if (!mainScenePanel.TryGetComponent<CoroutineRunner>(out coroutineRunner))
        {
            coroutineRunner = mainScenePanel.AddComponent<CoroutineRunner>();
        }
        startGame = StartGame;
    }

    public void StartGame()
    {
        coroutineRunner.StartCoroutine(MoveOutUI());
    }

    public IEnumerator ToNextScene()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("01Dream");
    }
    
    IEnumerator MoveOutUI()
    {
        Transform uiTransform;
        Vector3 worldPosition;
        Vector3 mainPanelPosition = mainScenePanel.transform.position;
        Vector3 targetPosition;
        for (int i = 0; i < mainScenePanel.transform.childCount; i++)
        {
            uiTransform = mainScenePanel.transform.GetChild(i);
            worldPosition = uiTransform.position;

            if (worldPosition.x <= mainPanelPosition.x)
            {
                targetPosition = new Vector3(worldPosition.x - mainScenePanel.mainPanelRect.width, worldPosition.y, worldPosition.z);
            }
            else
            {
                targetPosition = new Vector3(worldPosition.x + mainScenePanel.mainPanelRect.width, worldPosition.y, worldPosition.z);
            }
            uiTransform.DOMove(targetPosition, mainScenePanel.moveOutTime);
        }
        yield return new WaitForSeconds(mainScenePanel.moveOutTime);
        
        coroutineRunner.StartCoroutine(CreatRepeatImage());
    }
    
    IEnumerator CreatRepeatImage()
    {
        Vector3 spawnPosition = Vector3.zero;
        float intervalTime = mainScenePanel.intervalTime;
        for (int i = 0; i < mainScenePanel.repeatTimes; i++)
        {
            spawnPosition.x = Random.Range(mainScenePanel.mainPanelRect.xMin, mainScenePanel.mainPanelRect.xMax);
            spawnPosition.y = Random.Range(mainScenePanel.mainPanelRect.yMin, mainScenePanel.mainPanelRect.yMax);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Image newGameObject = GameObject.Instantiate(mainScenePanel.repeatImage, spawnPosition, rotation);
            newGameObject.transform.SetParent(mainScenePanel.transform, false);
            mainScenePanel.PlayAudio(mainScenePanel.appearImage);
            yield return new WaitForSeconds(intervalTime);
        }
        
        coroutineRunner.StartCoroutine(ToNextScene());
    } 
}
