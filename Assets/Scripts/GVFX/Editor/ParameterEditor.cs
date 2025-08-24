
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CustomEditor(typeof(BaseGVFXParameters), true)]
public class ParameterEditor : UnityEditor.Editor
{
        public override void OnInspectorGUI()
        {

                base.OnInspectorGUI();
        }

        private bool bufferIsRendering;
        private bool hasGetBuffer;
        public void OnEnable()
        {
                if (EditorApplication.isPlaying)
                {
                        return;
                }
                var param = target as BaseGVFXParameters;
                if (!param)
                {
                        return;   
                }
                var feature = GetRenderFeatureByType<GlitchVFXFeature>();
                if (!feature)
                {
                        return;   
                }
                var m = feature.settings.materialInfos.Find((m) => m.ID == param.GetGvfxType());
                bufferIsRendering = m.Rendering;
                hasGetBuffer = true;
        }

        public void OnSceneGUI()
        {
                if (EditorApplication.isPlaying)
                {
                        return;
                }
                var param = target as BaseGVFXParameters;
                if (!param)
                {
                        return;   
                }
                var feature = GetRenderFeatureByType<GlitchVFXFeature>();
                if (!feature)
                {
                     return;   
                }
                var m = feature.settings.materialInfos.Find((m) => m.ID == param.GetGvfxType());
                if (!hasGetBuffer)
                {
                        bufferIsRendering = m.Rendering;
                        hasGetBuffer = true;
                }
                if (EditorWindow.focusedWindow is AnimationWindow)
                {
                        if (m != null && hasGetBuffer && param.RenderingVFX)
                        {
                                m.Rendering = true;
                                param.SetMaterial(m);
                                param.UpdateParameters();
                        }
                }
                else
                {
                        //Debug.Log(2222);
                        if (hasGetBuffer)
                        {
                                m.Rendering = bufferIsRendering;
                        }
                }
        }

        public void OnDisable()
        {
                if (EditorApplication.isPlaying)
                {
                        return;
                }
                var param = target as BaseGVFXParameters;
                if (!param)
                {
                        return;   
                }
                var feature = GetRenderFeatureByType<GlitchVFXFeature>();
                if (!feature)
                {
                        return;   
                }
                var m = feature.settings.materialInfos.Find((m) => m.ID == param.GetGvfxType());
                if (hasGetBuffer)
                {
                        m.Rendering = bufferIsRendering;
                }
        }

        private  bool IsAnimationWindow(EditorWindow window)
        {
                if (window) return false;

                Type animationWindowType = Type.GetType("UnityEditor.AnimationWindow, UnityEditor");
                return window.GetType() == animationWindowType;
        }
        // 通过类型获取RenderFeature
        private  T GetRenderFeatureByType<T>() where T : ScriptableRendererFeature
        {
                var pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                if (pipelineAsset == null) return null;

                var renderer = pipelineAsset.scriptableRenderer;
                if (renderer == null) return null;
                
                PropertyInfo info = renderer.GetType().GetProperty("rendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);

                if (info.GetValue(renderer) is List<ScriptableRendererFeature> list)
                {
                        foreach (var feature in list)
                        {
     
                                if (feature is T targetFeature)
                                {
                                        return (T)feature;
                                }
                        }  
                }
                return null;
        }

}