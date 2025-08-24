using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames
{
    /// <summary>
    /// 重新弄了小游戏,想把分支做了的，但是没时间了，目前就是没有分支的版本
    /// </summary>
    public partial class MindStreamController : MonoBehaviour
    {
        private static MindStreamController _instance;
        public static MindStreamController Instance
        {
            get
            {
                if (!_instance)
                {
                    if (GameObject.Find("MindStreamController") is { } _go)
                    {
                        if (_go.TryGetComponent<MindStreamController>(out var _controller))
                        {
                            _instance = _controller;
                        }
                        else
                        {
                            _instance = _go.AddComponent<MindStreamController>();
                        }
                    }
                    var go = new GameObject("TestMindStream");
                    _instance = go.AddComponent<MindStreamController>();
                }
                return _instance;
            }
        }
        public void Update()
        {
           UpdatePlayerInput();
        }

        public void Start()
        {
            if (!_instance)
            {
                _instance = this;
            }
            //SetupBaseInfo();
        }

        public GameObject SingleCommendPrefab;

        public Transform ShowUIRectTransform;
        
        public CMDShowType currentShowType;
        
        public SingleCMD currentSingleCMD;

        public float cProcess;

        public bool canEndGame = false;
        
        public CanvasGroup UICanvasGroup;
        public static void HandleInstallMiniGame(List<string> param)
        {
            DialogueSystem.fs.Stop();
            Instance.SetupBaseInfo();
        }

        private void SetupBaseInfo()
        {
            cProcess = 0;
            UICanvasGroup.alpha = 1;
            UICanvasGroup.blocksRaycasts = true;  
            UICanvasGroup.interactable = true;    
            StartCoroutine(StartSetupTextCoroutine());
        }

        public void End()
        {
            Debug.Log(cProcess);
            UICanvasGroup.alpha = 0;
            UICanvasGroup.blocksRaycasts = false;  // 取消遮挡，允许点击穿透
            UICanvasGroup.interactable = false;    // 禁用交互（可选）
        }

        private bool oldIsHold;
        
        private IEnumerator StartSetupTextCoroutine()
        {
            foreach (var valueTuple in StartTexts)
            {
                if (oldIsHold)
                {
                    GameObject.DestroyImmediate(ShowUIRectTransform.transform.GetChild(0).gameObject);
                    oldIsHold = false;
                }
                CreateCMD(valueTuple.Item1, valueTuple.Item2);

                //遇到空格额外多等一会
                if (valueTuple.Item1 == CMDShowType.HoldOn)
                {
                    oldIsHold = true;
                    yield return new WaitForSeconds(3f);
                }
                
                if (string.IsNullOrEmpty(valueTuple.Item2))
                {
                    yield return new WaitForSeconds(0.8f);
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
                }
               
            }
            //然后是正常的处理
            StartCoroutine(ProcessFristTexts());
        }
        private bool finishedDialogue = false;
        private IEnumerator ProcessFristTexts()
        {
            foreach (var tuple in fristTryTexts)
            {

                if (oldIsHold)
                {
                    GameObject.DestroyImmediate(ShowUIRectTransform.transform.GetChild(0).gameObject);
                    oldIsHold = false;
                }

                switch (tuple.Item1)
                {
                    case CMDShowType.Input:
                    {
                        CreateCMD(tuple.Item1, tuple.Item2);
                        yield return new WaitUntil(() =>currentSingleCMD.canSkipToNext()) ;
                        //计算进度
                        cProcess += currentSingleCMD.GetPercentOfProgress() * tuple.Item4;
                    }
                        break;
                    //正常按时间就好了
                    case CMDShowType.Output:
                        CreateCMD(tuple.Item1, tuple.Item2);
                        yield return new WaitForSeconds(tuple.Item3);
                        break;
                    //正常按时间就好了
                    case CMDShowType.HoldOn:
                        CreateCMD(tuple.Item1, tuple.Item2);
                        oldIsHold = true;
                        yield return new WaitForSeconds(tuple.Item3);
                        break;
                    //skip
                    case CMDShowType.Dialog:
                        CreateDialog(tuple.Item2, tuple.Item3);
                        yield return new WaitUntil(() => finishedDialogue);
                        break;
                    default:
                        yield return null;
                        break;
                };
            }


            yield return null;
            DialogueSystem.fs.Resume();
            End();
        }

        private void CreateCMD(CMDShowType showType, string param)
        {
            var instance = GameObject.Instantiate(SingleCommendPrefab, ShowUIRectTransform);
            if (instance.TryGetComponent(typeof(SingleCMD),out var cmd) && cmd is SingleCMD singleCMD)
            {
                singleCMD.Setup(showType,param);
                currentShowType = showType;
                currentSingleCMD = singleCMD;
            }
            instance.transform.SetAsFirstSibling();
        }

        private void UpdatePlayerInput()
        {
            if (!currentSingleCMD)
            {
                return;
            }
            //获取26个字母的输入
            if (Input.GetKeyDown(KeyCode.A))
                currentSingleCMD.Input('a');
            if (Input.GetKeyDown(KeyCode.B))
                currentSingleCMD.Input('b');
            if (Input.GetKeyDown(KeyCode.C))
                currentSingleCMD.Input('c');
            if (Input.GetKeyDown(KeyCode.D))
                currentSingleCMD.Input('d');
            if (Input.GetKeyDown(KeyCode.E))
                currentSingleCMD.Input('e');
            if (Input.GetKeyDown(KeyCode.F))
                currentSingleCMD.Input('f');
            if (Input.GetKeyDown(KeyCode.G))
                currentSingleCMD.Input('g');
            if (Input.GetKeyDown(KeyCode.H))
                currentSingleCMD.Input('h');
            if (Input.GetKeyDown(KeyCode.I))
                currentSingleCMD.Input('i');
            if (Input.GetKeyDown(KeyCode.J))
                currentSingleCMD.Input('j');
            if (Input.GetKeyDown(KeyCode.K))
                currentSingleCMD.Input('k');
            if (Input.GetKeyDown(KeyCode.L))
                currentSingleCMD.Input('l');
            if (Input.GetKeyDown(KeyCode.M))
                currentSingleCMD.Input('m');
            if (Input.GetKeyDown(KeyCode.N))
                currentSingleCMD.Input('n');
            if (Input.GetKeyDown(KeyCode.O))
                currentSingleCMD.Input('o');
            if (Input.GetKeyDown(KeyCode.P))
                currentSingleCMD.Input('p');
            if (Input.GetKeyDown(KeyCode.Q))
                currentSingleCMD.Input('q');
            if (Input.GetKeyDown(KeyCode.R))
                currentSingleCMD.Input('r');
            if (Input.GetKeyDown(KeyCode.S))
                currentSingleCMD.Input('s');
            if (Input.GetKeyDown(KeyCode.T))
                currentSingleCMD.Input('t');
            if (Input.GetKeyDown(KeyCode.U))
                currentSingleCMD.Input('u');
            if (Input.GetKeyDown(KeyCode.V))
                currentSingleCMD.Input('v');
            if (Input.GetKeyDown(KeyCode.W))
                currentSingleCMD.Input('w');
            if (Input.GetKeyDown(KeyCode.X))
                currentSingleCMD.Input('x');
            if (Input.GetKeyDown(KeyCode.Y))
                currentSingleCMD.Input('y');
            if (Input.GetKeyDown(KeyCode.Z))
                currentSingleCMD.Input('z');
            if (Input.GetKeyDown(KeyCode.Comma) && Input.GetKey(KeyCode.LeftShift)) //偷懒
                currentSingleCMD.Input('<');
            if (Input.GetKeyDown(KeyCode.Period) && Input.GetKey(KeyCode.LeftShift)) //偷懒
                currentSingleCMD.Input('>');
            if (Input.GetKeyDown(KeyCode.Equals) && Input.GetKey(KeyCode.LeftShift)) //偷懒
                currentSingleCMD.Input('+');
            if (Input.GetKeyDown(KeyCode.Equals)) //偷懒
                currentSingleCMD.Input('=');
            if (Input.GetKeyDown(KeyCode.Minus)) //偷懒
                currentSingleCMD.Input('-');
            if (Input.GetKeyDown(KeyCode.Space)) //偷懒
                currentSingleCMD.Input(' ');
            if (Input.GetKeyDown(KeyCode.Backspace)) //删除
                currentSingleCMD.BackSpace();
        }

        private void CreateDialog(string text, float time)
        {
            finishedDialogue = false;
            StartCoroutine(ProcessDialog(text, time));
        }

        private IEnumerator ProcessDialog(string text, float time)
        {
            //为什么这个Flower没有外部干涉的指令？，还得自己去暴露参数
            yield return DialogueSystem.fs.CmdFunc_dialogShow_Task(new List<string>(){"10"});
            DialogueSystem.fs.UpdateText(text);
            yield return new WaitForSeconds(time);
            yield return DialogueSystem.fs.CmdFunc_dialogHide_Task(new List<string>(){"10"});
            finishedDialogue = true;
        }
    }
}