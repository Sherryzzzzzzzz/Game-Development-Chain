using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Flower
{
    public class MiniGameManager : Singleton<MiniGameManager>
    {

        // 小游戏预制体引用（在Inspector中赋值）
        public GameObject QTEPrefab;//(记得是选择Asset中的预制体)

        public FlowerSystem flowerSystem;
        private Action<bool> miniGameCallback;
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            flowerSystem = FindObjectOfType<FlowerSystem>();
        }
        // 启动小游戏的公共接口
        public void StartMiniGame(string gameType, Action<bool> callback)
        {
            miniGameCallback = callback;
            flowerSystem.Stop(); // 暂停流程

            switch (gameType.ToLower())
            {
                case "qte":
                    StartCoroutine(StartQTEGame());
                    break;
                default:
                    Debug.LogError($"Unknown mini-game type: {gameType}");
                    flowerSystem.Resume();
                    break;
            }
        }

        // 小游戏完成回调
        public void EndMiniGame(bool success)
        {
            if (miniGameCallback != null)
            {
                miniGameCallback(success);
            }
            flowerSystem.Resume(); // 恢复流程
        }

        #region 小游戏具体实现
        private IEnumerator StartQTEGame()
        {
            GameObject QTEGame = Instantiate(QTEPrefab);
            QTEManager gameController = QTEGame.GetComponentInChildren<QTEManager>();

            gameController.StartQTE();

            yield return new WaitUntil(() => gameController.IsCompleted);

            bool success = gameController.IsSuccess;
            Destroy(QTEGame);

            flowerSystem.SetVariable("qte_result", success ? "success" : "fail");
            EndMiniGame(success);
        }
        #endregion
    }
}