using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class RGBSplitParameters : BaseGVFXParameters
{
        private static readonly int SplitAmout = Shader.PropertyToID("_SplitAmout");
        private static readonly int SplitDirection = Shader.PropertyToID("_SplitDirection");
        private static readonly int SplitIntensity = Shader.PropertyToID("_SplitIntensity");
        private static readonly int UseDynamicSplitVector = Shader.PropertyToID("_UseDynamicSplitVector");
        
        private AnimationClipPlayable playable_clip;
        
        public override GvfxType GetGvfxType()
        {
                return GvfxType.RGBSplit;
        }
        

        public override void UpdateParameters()
        {
                if (Info !=null && Info.Material)
                {
                        Info.Material.SetVector(SplitDirection,_SplitDirection);
                        Info.Material.SetFloat(SplitIntensity,_SplitIntensity);
                        Info.Material.SetFloat(SplitAmout,_SplitAmout);
                        Info.Material.SetFloat(UseDynamicSplitVector, _UseDynamicSplitVector ? 1 : 0);
                }
        }

        public Vector2 _SplitDirection = Vector2.right;
        public float _SplitIntensity = 1;
        public float _SplitAmout = 1;
        public bool _UseDynamicSplitVector= false;

        public void Awake()
        {
                
        }

        public void Start()
        {
                
        }

        public override void Update()
        {
                base.Update();
        }

        public override void SceneApply(List<string> param)
        {
               //直接改成Rendering的值就好了
               RenderingVFX = true;
        }

        public override void PlayerAnimation(List<string> param)
        {
                //加载动画 应该就要个地址就好了
                if (param.Count == 0)
                {
                        Debug.Log("没有动画地址");
                        return;
                }

                var clip = LoadResource<AnimationClip>(param[0]);
                if (clip != null)
                {
                        PlayAnimClip(clip);
                }
        }

        public override void SetParam(List<string> param)
        {
                try
                {
                        _UseDynamicSplitVector = param[0] == "true";
                        if (_UseDynamicSplitVector)
                        {
                                try
                                {
                                        float x = float.Parse(param[1]);
                                        float y = float.Parse(param[2]);
                                        _SplitDirection = new Vector2(x, y);
                                }
                                catch (Exception)
                                {
                                }
                        }
                        else
                        {
                                _SplitIntensity = float.Parse(param[1]);
                                _SplitAmout = float.Parse(param[2]);
                        }
                }
                catch (Exception e)
                {
                        throw new Exception($"Invalid parameters.\n{e}");
                }

                RenderingVFX = true;
        }

        public override void StopVFX()
        {
               //动画和设置，直接Rendering = False了
               StopAnim();
               
               RenderingVFX = false;
               Info.Rendering = false;
        }
}