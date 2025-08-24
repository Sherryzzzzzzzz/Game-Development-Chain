using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;
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
        //change
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
            if (uiTransform.GetComponent<Button>())
            {
                uiTransform.GetComponent<Button>().interactable = false;
            }
            if (uiTransform.GetComponent<UIMoveSet>())
            {
                uiTransform.GetComponent<UIMoveSet>().enabled = false;
            }
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
        List<GameObject> list = new List<GameObject>();
        Vector3 spawnPosition = Vector3.zero;
        float intervalTime = mainScenePanel.intervalTime;
        for (int i = 0; i < mainScenePanel.repeatTimes; i++)
        {
            spawnPosition.x = Random.Range(mainScenePanel.mainPanelRect.xMin, mainScenePanel.mainPanelRect.xMax);
            spawnPosition.y = Random.Range(mainScenePanel.mainPanelRect.yMin, mainScenePanel.mainPanelRect.yMax);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Image newGameObject = GameObject.Instantiate(mainScenePanel.repeatImage, spawnPosition, rotation);
            // 获取 RectTransform 组件
            RectTransform rectTransform = newGameObject.GetComponent<RectTransform>();

            // 1. 设置锚点为中心
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f); // 锚点最小值（左下角）
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f); // 锚点最大值（右上角）

            // 2. 设置宽高为 550x150
            rectTransform.sizeDelta = new Vector2(550f, 150f);
            list.Add(newGameObject.gameObject);
            newGameObject.transform.SetParent(mainScenePanel.transform, false);
            mainScenePanel.PlayAudio(mainScenePanel.appearImage);
            yield return new WaitForSeconds(intervalTime);
        }
        for(int i = 0;i < list.Count; i++)
        {
            Vector3 startPos = list[i].transform.position;
            float offset = Random.Range(-100, 100);
            Vector3 endPos = new Vector3(list[i].transform.position.x + offset, -500, list[i].transform.position.z);
            Vector3[] path = new Vector3[] { startPos, endPos };
            list[i].transform.DOPath(path, 2f, PathType.CatmullRom) // CatmullRom或Linear都可以
                 .SetEase(Ease.InQuad);
        }
        list.Clear();
        coroutineRunner.StartCoroutine(ToNextScene());
    } 
}
