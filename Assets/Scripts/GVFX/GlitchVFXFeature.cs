using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchVFXFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class PostSettings {
        public bool RenderingInEditor = false;
        
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        public List<MaterialInfo> materialInfos = new List<MaterialInfo>();
        
    }
    public class CustomRenderPass : ScriptableRenderPass
    {
        public MaterialList materialList;

        //用来存 frame buffer
        RTHandle _cameraDepth;
        RTHandle _cameraColor;
        
        RTHandle rtHandle;
        RTHandle rtHandle2;
        
        // RenderTextureDescriptor 用来设置纹理属性     
         RenderTextureDescriptor _descriptor;

         public bool RenderingInEditor;
        public CustomRenderPass()
        {
        }
        
        public void Setup(List<MaterialInfo> materialInfos,bool renderingInEditor)
        {
            materialList = new MaterialList();
            materialList.Set(materialInfos);
            RenderingInEditor = renderingInEditor;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _cameraColor = renderingData.cameraData.renderer.cameraColorTargetHandle;
            _cameraDepth = renderingData.cameraData.renderer.cameraDepthTargetHandle;
            _descriptor = renderingData.cameraData.cameraTargetDescriptor;
            _descriptor.depthBufferBits = 0;
            _descriptor.msaaSamples = 1;
            //_descriptor.width = renderingData.cameraData.camera.scaledPixelWidth; 
            //_descriptor.height = renderingData.cameraData.camera.scaledPixelHeight; 
            RenderingUtils.ReAllocateIfNeeded( ref rtHandle, _descriptor, name:"_tempRT" );
            RenderingUtils.ReAllocateIfNeeded( ref rtHandle2, _descriptor, name:"_tempRT2" );
            
            ConfigureTarget(rtHandle);
            ConfigureTarget(rtHandle2);
            ConfigureTarget( _cameraColor,_cameraDepth);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            bool trySetupManager = true;
#if UNITY_EDITOR
            //不确定打包后是否会出bbug
            if (!EditorApplication.isPlaying)
            {
              trySetupManager = false;
              //return;
            }

            if (!RenderingInEditor && !EditorApplication.isPlaying)
            {
                return;   
            }
#endif

            if (trySetupManager)
            {
                GVFXManager.Instance.TrySetupAllMaterials(materialList);
            }
            
            CommandBuffer cmd = CommandBufferPool.Get();
            cmd.name = "GVFX pass";
            

           
            cmd.Blit(_cameraColor, rtHandle);

            bool swapRT = true;
            foreach (var materinfo in materialList)
            {
                if (materinfo.Material == null) continue;
                if (!materinfo.Rendering)
                {
                    continue;
                }
                var source = swapRT ? rtHandle : rtHandle2;
                var destination = swapRT ? rtHandle2 : rtHandle;
                // 应用材质到当前目标
                cmd.Blit(source,destination  , materinfo.Material,0);
      
                // 更新源为当前结果，准备下一次处理
                swapRT = !swapRT;
               // CoreUtils.DrawFullScreen(cmd,materinfo.Material);
            }
           
            //cmd.Blit(, _cameraColor);
            Blitter.BlitCameraTexture(cmd,  swapRT ? rtHandle : rtHandle2, _cameraColor);
            context.ExecuteCommandBuffer(cmd);
            // 5. 执行所有命令
            CommandBufferPool.Release(cmd);
        }
        
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
          //  Dispose();
          //  rtHandle = null;
          //  rtHandle2 = null;
        }

        public void Dispose()
        {
            rtHandle?.Release();
            rtHandle2?.Release();
        }
    }


    public PostSettings settings = new PostSettings();
    public CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
        m_ScriptablePass.Setup(settings.materialInfos,settings.RenderingInEditor);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }

}