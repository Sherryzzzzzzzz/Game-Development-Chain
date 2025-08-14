using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockGlitchParameters : BaseGVFXParameters
{
        private static readonly int Block1Size = Shader.PropertyToID("_Block1Size");
        private static readonly int Block2Size = Shader.PropertyToID("_Block2Size");
        private static readonly int Block1Instensity = Shader.PropertyToID("_Block1Instensity");
        private static readonly int Block2Instensity = Shader.PropertyToID("_Block2Instensity");
        private static readonly int Block1Offest = Shader.PropertyToID("_Block1Offest");
        private static readonly int Block2Offest = Shader.PropertyToID("_Block2Offest");
        private static readonly int Instensity = Shader.PropertyToID("_Instensity");
        private static readonly int BigOffestSize = Shader.PropertyToID("_BigOffestSize");
        public Vector2 _Block1Size = new Vector2(13,5);
        public Vector2 _Block2Size= new Vector2(6,11);

        public float _Block1Instensity = 0.5f;
        public float _Block2Instensity= 0.5f;

        public Vector2 _Block1Offest = new Vector2(0.27f,-0.09f);
        public Vector2 _Block2Offest = new Vector2(0.12f,-0.21f);

        public float _Instensity =1;

        public Vector2 _BigOffestSize = new Vector2(3, 3);
        public override GvfxType GetGvfxType()
        {
                return GvfxType.BlockGlitch;
        }

        public override void UpdateParameters()
        {
                if (Info != null && Info.Material)
                {
                        Info.Material.SetVector(Block1Size,_Block1Size);
                        Info.Material.SetVector(Block2Size,_Block2Size);
                        
                        Info.Material.SetFloat(Block1Instensity,_Block1Instensity);
                        Info.Material.SetFloat(Block2Instensity,_Block2Instensity);
                        
                        Info.Material.SetVector(Block1Offest,_Block1Offest);
                        Info.Material.SetVector(Block2Offest,_Block2Offest);
                        
                        Info.Material.SetFloat(Instensity,_Instensity);
                        
                        Info.Material.SetVector(BigOffestSize,_BigOffestSize);
                }
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
                //7个参数，这也太多了吧
                try
                {
                        try {
                                float x = float.Parse(param[0]);
                                float y = float.Parse(param[1]);
                                _Block1Size = new Vector2(x, y);
                        }catch (Exception) { }
                        try {
                                float x = float.Parse(param[2]);
                                float y = float.Parse(param[3]);
                                _Block2Size = new Vector2(x, y);
                        }catch (Exception) { }

                        _Block1Instensity = float.Parse(param[4]);
                        _Block2Instensity = float.Parse(param[5]);
                        try {
                                float x = float.Parse(param[6]);
                                float y = float.Parse(param[7]);
                                _Block1Offest = new Vector2(x, y);
                        }catch (Exception) { }
                        try {
                                float x = float.Parse(param[8]);
                                float y = float.Parse(param[9]);
                                _Block2Offest = new Vector2(x, y);
                        }catch (Exception) { }
                        
                        _Instensity = float.Parse(param[10]);
                        
                        try {
                                float x = float.Parse(param[11]);
                                float y = float.Parse(param[12]);
                                _BigOffestSize = new Vector2(x, y);
                        }catch (Exception) { }

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