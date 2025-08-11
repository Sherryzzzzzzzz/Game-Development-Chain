using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flower
{
    /// 你说得对，但是我为了手搓点小游戏进去这个gal里面我最终选择手动扩展Flower,显然过程借助了不少方法(包括d指导),
    /// 
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 记得看readme
    /// 
    /// 后面棒的有心人可以稍微看看这个Extension还有MinigameManager还有QTEManager,也许可以为整个游戏的多样化有点帮助...
    /// (欸这QTE是否有点像音游...你们臭打邦的真是kimoi)
    /// 起初拿到这棒的内容讲真有点绝望,作为一个文笔废物的不行的臭程序居然要做galgame，并且我看了近一整天的代码(原谅我去翻git记录)才发现前几棒已经集成好了FlowerSystem
    /// 一看到前几棒基本都是搬素材,填剧情,读FlowerSys文件学指令,我就在思考自己能不能像个程序员一点,针对整个游戏的主要核心FlowerSys来一点有趣的扩展
    /// 但是由于FlowerSys的神人指令读取方式(逐行读取),我尝试过制作标签跳转(类似goto代码)实现类gal分支的方案,结果失败了
    /// 然后给这个系统做一个文本预处理的功能实在是太繁琐,作为游戏接龙的一部分真的太不值得了
    /// QTE游戏在07场景中使用了,不影响结局但是作为一个演出行为可能不错(我的素材有点出戏)
    /// QTE的使用方法就是从文本中写[minigame,qte,qte_result]这一行即可
    /// 由于FlowerSystem是游戏内实时创建的对象,没办法在inspector中直接拉进去我的代码里，我选择修改了DialogueSystem,前几棒都有对此进行修改,可以参考一下
    /// 最后就搓出来这一坨玩意,希望能够为后面的大神们的内容有所帮助,我的工期只有两天,东西做的太少了...
    /// 最后的最后,游戏接龙下次我真的有点想禁止galgame参赛了...如果选择了galgame,真的会很限制对于游戏本身的思路,对游戏内容的开发接龙感觉就变成了故事接龙了...
    /// 
    /// 
    /// 
    /// 从第四棒(我)开始总结的测试维护指南:
    /// 1.单独场景测试剧情播放需要从场景01Dream将DialogSystem和HistoryLogCanvas复制一份到需要的场景中才可以测试
    /// 2.遇到场景无法跳转(如03跳不到04)请先检查左上角File-->BuildSettings-->Scenes In Build中是否有04这个场景
    /// 3.(待补充)
    /// 4.我好绝望

    public class FlowerSystemExtension : MonoBehaviour
    {
        public FlowerSystem flowerSystem;

        void Start()
        {
            // 注册小游戏命令
            flowerSystem.RegisterCommand("minigame", (List<string> _params) => flowerSystem.StartCoroutine(StartMiniGameCoroutine(_params)));
        }

        private IEnumerator StartMiniGameCoroutine(List<string> _params)
        {
            if (_params.Count < 1)
            {
                Debug.LogError("minigame command requires at least 1 parameter");
                yield break;
            }

            string gameType = _params[0];
            string resultVar = _params.Count > 1 ? _params[1] : $"{gameType}_result";
            //string successBlock = _params.Count > 2 ? _params[2] : null;
            //string failBlock = _params.Count > 3 ? _params[3] : null;

            bool? result = null;
            MiniGameManager.instance.StartMiniGame(gameType, (success) => {
                result = success;

                // 设置结果变量
                flowerSystem.SetVariable(resultVar, success ? "success" : "fail");

                // 触发分支跳转(感觉没用)
                // 是的,flower就是做不到
                //if (success && !string.IsNullOrEmpty(successBlock))
                //{
                //    flowerSystem.InvokeSpecialCommand($"jump,{successBlock}");
                //}
                //else if (!success && !string.IsNullOrEmpty(failBlock))
                //{
                //    flowerSystem.InvokeSpecialCommand($"jump,{failBlock}");
                //}
            });

            // 等待小游戏完成
            yield return new WaitUntil(() => result.HasValue);
        }
    }
}