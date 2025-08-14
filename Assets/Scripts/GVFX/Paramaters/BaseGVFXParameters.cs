using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public abstract class BaseGVFXParameters : MonoBehaviour
{
    public AnimationLayerMixerPlayable playableMixer;
    public PlayableGraph playableGraph;
    public AnimationMixerPlayable selfMixer;
    public int index;
    
    public List<ParameterAnimState> animStates = new List<ParameterAnimState>();

    public bool RenderingVFX;
    
    public MaterialInfo Info;
    
    private List<ParameterAnimState> removelist = new List<ParameterAnimState>();
    
    private Animator animator;
    public void SetPlayable(Animator animator,AnimationLayerMixerPlayable playableMixer,PlayableGraph playableGraph,int index)
    {
        this.playableMixer = playableMixer;
        this.playableGraph = playableGraph;
        this.index = index;
        this.animator = animator;
        
        selfMixer =  AnimationMixerPlayable.Create(playableGraph,1);
        playableGraph.Connect(selfMixer, 0, playableMixer, index);
        this.playableMixer.SetInputWeight(index, 1);
    }
    
    public abstract GvfxType GetGvfxType();

    public virtual void SetMaterial(MaterialInfo Info)
    {
        this.Info = Info;
    }
    
    public abstract void UpdateParameters();

    public void PlayAnimClip(AnimationClip clip,float time =0)
    {
        RenderingVFX = true;
        animStates.ForEach(s=>s.Dispose());
        animStates.Clear();
        var newState = new ParameterAnimState(animator,selfMixer, playableGraph, clip, 0, 1, time);
        animStates.Add(newState);
    }

    public void StopAnim()
    {
        animStates.ForEach(s=>s.Dispose());
        animStates.Clear();
    }

    public virtual void Update()
    {
        Info.Rendering = RenderingVFX;
        
        animStates.ForEach(s =>
        {
            s.Update(Time.deltaTime);
            if (s.animState == ParameterAnimState.AnimStates.OnceEnd)
            {
                s.Dispose();
                removelist.Add(s);
            }
        });

        foreach (var state in removelist)
        {
            animStates.Remove(state);
            RenderingVFX = false;
        }
        removelist.Clear();
        
        UpdateParameters();
    }
    
    //各种命令的解析，真是恶心完了
    public abstract void SceneApply(List<string> param);

    public abstract void PlayerAnimation(List<string> param);

    public abstract void SetParam(List<string> param);

    public abstract void StopVFX();
    
    
    public T LoadResource<T>(string path) where T:UnityEngine.Object{
        var obj = Resources.Load<T>(path) as T;
        if(obj!=null){
            return obj;
        }else{
            throw new Exception($"Resource - {path} not exists.");
        }
    }
}