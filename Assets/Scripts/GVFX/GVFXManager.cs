using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

//处理后处理效果
//主要实现了些Glitch效果，感觉挺适合做这个项目的
//主要效果有RGB分离和图块偏移两个，支持直接设置值：
//已接入Flower 包含以下指令
//1.SceneApply [后处理名称] 需要在场景中先设置对应Parameter中的值，使用这个指令就可以启用效果并采用预设的值
//2.playAnim [后处理名称] [动画文件path] 通过播放动画来启用效果
//3.SetParam [后处理名称] [一堆参数，推荐去每个Parameter单独查看] 设置参数来启用效果
//4.stopVFX [后处理名称] 立刻停止效果
//5.stopAll 停止所有的效果
//如果想加新的后处理 可以按以下步骤来：
//1.写一个unlit的urp shader ，只要有一个属性是"_MainTex" 就可通过这个Texture获取屏幕像素
//2.把这个shader对应的material放入Urp setting的 GlitchVFXFeature 的MaterialInfo中
//3.在GvfxType 下添加对应的枚举值
//4.继承<BaseGVFXParameters> 写将参数写入材质的方法
//5.记得补充GVFXManager下的SelectVFX ，否则指令找不到对应的特效

//主义事项：
//如果要创建动画，推荐每个Parameter单独一个动画
//GVFXManager 非必要挂载到场景里，可以自己加载出来
//如果Inspector在GVFXManager下退出 Play模式，会有报错，但其实不影响，重新选个其他的GameObject就好了

//Flower 感觉半坨，这个东西感觉像个临时急用的，很多地方用的很难受，主要也没人研究 (｡•́︿•̀｡) 希望后面有人可以做分支出来
public class GVFXManager : MonoBehaviour
{
    public static List<(GvfxType, Type)> gvfxParameters = new List<(GvfxType, Type)>()
    {
        (GvfxType.RGBSplit, typeof(RGBSplitParameters)),
        (GvfxType.BlockGlitch,typeof(BlockGlitchParameters))
    };
    
    #region 单例

    private static GVFXManager _instance;
        
    public static GVFXManager Instance
    {
        get
        {
            if (!_instance)
                CreateInstance();
            return _instance;
        }
    }
    
    private static void CreateInstance()
    {
        var componentInScene = GameObject.Find("GVFXManager");
        if (componentInScene && componentInScene.GetComponent<GVFXManager>() is { } gfxManager)
        {
            _instance = gfxManager;
            foreach (var gvfxParameter in gvfxParameters)
            {
                if (componentInScene.TryGetComponent(gvfxParameter.Item2,out var result) && result is BaseGVFXParameters igp)
                {
                    _instance._parameters[gvfxParameter.Item1] = igp;
                }
                else
                {
                    var p = _instance.gameObject.AddComponent(gvfxParameter.Item2);
                    if (p is BaseGVFXParameters _add)
                    {
                        _instance._parameters[gvfxParameter.Item1] = _add;
                    }
                }
            }

            if (_instance.gameObject.TryGetComponent(typeof(Animator),out var anim) && anim is Animator animator)
            {
                _instance._animator = animator;
            }
            else
            {
                _instance._animator = _instance.gameObject.AddComponent(typeof(Animator)) as Animator;
            }
            //GameObject.DontDestroyOnLoad(componentInScene);
            return;
        }
            
        var go = new GameObject("GVFXManager");
        //GameObject.DontDestroyOnLoad(go);
        _instance = go.AddComponent<GVFXManager>();
        foreach (var gvfxParameter in gvfxParameters)
        {
            var p = _instance.gameObject.AddComponent(gvfxParameter.Item2);
            if (p is BaseGVFXParameters _add)
            {
                _instance._parameters[gvfxParameter.Item1] = _add;
            }
        }

        {
            if (_instance.gameObject.TryGetComponent(typeof(Animator),out var anim) && anim is Animator animator)
            {
                _instance._animator = animator;
            }
            else
            {
                _instance._animator = _instance.gameObject.AddComponent(typeof(Animator)) as Animator;
            }
        }
    }

    #endregion

    [SerializeField]
    private MaterialList _materialList;
    
    private Dictionary<GvfxType,BaseGVFXParameters> _parameters = new Dictionary<GvfxType, BaseGVFXParameters>();
    
    private Animator _animator;
    
    
    private PlayableGraph _graph;
    private AnimationPlayableOutput output;
    private AnimationLayerMixerPlayable mixer;
    
    private bool hasInitialized = false;

    public void Start()
    {
        
    }
    

    public void TrySetupAllMaterials(MaterialList list)
    {
        _animator.runtimeAnimatorController = null;
        if (hasInitialized)
        {
            return;
        }
        hasInitialized = true;
        _materialList = list;

        foreach (var materialInfo in _materialList)
        {
            if (_parameters.TryGetValue((GvfxType)materialInfo.ID, out var result))
            {
                result.SetMaterial(materialInfo);
            }
        }
        
        
        _graph = PlayableGraph.Create();
        output = AnimationPlayableOutput.Create(_graph, "AnimationOutput", GetComponent<Animator>());
        mixer = AnimationLayerMixerPlayable.Create(_graph,_materialList.Count);
        output.SetSourcePlayable(mixer);

        for (int i = 0; i < _parameters.Values.Count; i++)
        {
            var parameter = _parameters.Values.ToList()[i];
            // 创建ClipPlayable
           
            parameter.SetPlayable(_animator,mixer,_graph,i);
            // 连接到混合器
           // _graph.Connect(clipPlayable, 0, mixer, i);
                
            // 设置初始权重
            mixer.SetInputWeight(i,1);
                
            // 设置播放状态
        }

        _graph.Play();
    }

    public static void HandleGVFXCommend(List<string> param)
    {
        if (param.Count == 0)
        {
            Debug.LogError("[gvfx]无明确指令");
            return;
        }
        switch (param[0])
        {
            //从场景中预制的组件中读取数据
            case "SceneApply":
                param.RemoveAt(0);
                Instance.SceneApply(param);
                break;
            //播放动画
            case "playAnim":
                param.RemoveAt(0);
                Instance.PlayerAnimation(param);
                break;
            //直接设置对应后处理效果的参数
            case "SetParam":
                param.RemoveAt(0);
                Instance.SetParam(param);
                break;
            //停止对应的后处理效果
            case "stopVFX":
                param.RemoveAt(0);
                Instance.StopVFX(param);
                break;
            //停止所有后处理效果
            case "stopAll":
                param.RemoveAt(0);
                Instance.StopAll();
                break;
            default:
                Debug.Log("[gvfx]未知指令");
                break;
        }
    }

    public void SceneApply(List<string> param)
    {
        var targetVFX = SelectVFX(param);
        if (targetVFX)
        {
            targetVFX.SceneApply(param);
        }
    }

    public void PlayerAnimation(List<string> param)
    {
        var targetVFX = SelectVFX(param);
        if (targetVFX)
        {
            targetVFX.PlayerAnimation(param);
        }
    }

    public void SetParam(List<string> param)
    {
        var targetVFX = SelectVFX(param);
        if (targetVFX)
        {
            targetVFX.SetParam(param);
        }
    }

    public void StopVFX(List<string> param)
    {
        var targetVFX = SelectVFX(param);
        if (targetVFX)
        {
            targetVFX.StopVFX();
        }
    }

    public void StopAll()
    {
        foreach (var parameter in _parameters)
        {
            parameter.Value.StopVFX();
        }
    }

    public BaseGVFXParameters SelectVFX(List<string> param)
    {
        if (param.Count == 0)
        {
            Debug.LogError("没有VFX名称");
            return null;
        }
        //取出第一个参数
        string vfxType = param[0];
        param.RemoveAt(0);

        switch (vfxType)
        {
            case "RGBSplit":
                return _parameters[GvfxType.RGBSplit];
            case "BlockGlitch":
                return _parameters[GvfxType.BlockGlitch];
        }
        return null;
    }

    public void OnDestroy()
    {
        StopAll();
        _graph.Destroy();
    }
}