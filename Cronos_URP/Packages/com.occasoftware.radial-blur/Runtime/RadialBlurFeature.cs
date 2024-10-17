using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.RadialBlur.Runtime
{
    public class RadialBlurFeature : ScriptableRendererFeature
    {
        class RadialBlurPass : ScriptableRenderPass
        {
            private const string bufferName = "Radial Blur Pass";
            private const string radialBlurTargetName = "Radial Blur Target";
            private const string radialBlurShaderId = "OccaSoftware/RadialBlur/Blur";
            private Material radialBlurMaterial = null;

            private RTHandle source;
            private RTHandle target;

            private RadialBlurPostProcess radialBlur = null;

            public RadialBlurPass()
            {
                target = RTHandles.Alloc(Shader.PropertyToID(radialBlurTargetName), name: radialBlurTargetName);
            }

            internal bool RegisterStackComponent()
            {
                radialBlur = VolumeManager.instance.stack.GetComponent<RadialBlurPostProcess>();

                if (radialBlur == null)
                    return false;

                return radialBlur.IsActive();
            }

            public void SetTarget(RTHandle source)
            {
                this.source = source;
            }

            internal bool SetupMaterial()
            {
                if (radialBlurMaterial == null)
                {
                    Shader shader = Shader.Find(radialBlurShaderId);
                    if (shader != null)
                    {
                        radialBlurMaterial = CoreUtils.CreateEngineMaterial(radialBlurShaderId);
                    }
                }

                return radialBlurMaterial != null;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
                descriptor.width = Mathf.Max(1, descriptor.width);
                descriptor.height = Mathf.Max(1, descriptor.height);
                descriptor.msaaSamples = 1;
                descriptor.depthBufferBits = 0;
                descriptor.sRGB = false;

                RenderingUtils.ReAllocateIfNeeded(ref target, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: radialBlurTargetName);

            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {

                CommandBuffer cmd = CommandBufferPool.Get(bufferName);
                ConfigureTarget(source);

                SetShaderParams();
                cmd.SetGlobalTexture("_Source", source);
                Blitter.BlitCameraTexture(cmd, source, target, radialBlurMaterial, 0);
                Blitter.BlitCameraTexture(cmd, target, source);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {

            }

            private void SetShaderParams()
            {
                radialBlurMaterial.SetVector(RadialBlurShaderParams.center, radialBlur.GetCenter());
                radialBlurMaterial.SetFloat(RadialBlurShaderParams.intensity, radialBlur.GetIntensity());
                radialBlurMaterial.SetFloat(RadialBlurShaderParams.delay, radialBlur.GetDelay());
                radialBlurMaterial.SetInt(RadialBlurShaderParams.sampleCount, radialBlur.GetSampleCount());
            }
        }

        RadialBlurPass radialBlurPass;

        public override void Create()
        {
            radialBlurPass = new RadialBlurPass();
            radialBlurPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.cameraType == CameraType.Reflection)
                return;

            if (renderingData.cameraData.camera.cameraType == CameraType.Preview)
                return;

            if (!renderingData.cameraData.postProcessEnabled)
                return;

            if (!radialBlurPass.RegisterStackComponent())
                return;

            if (!radialBlurPass.SetupMaterial())
                return;

            renderer.EnqueuePass(radialBlurPass);
        }


        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            radialBlurPass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
            radialBlurPass.SetTarget(renderer.cameraColorTargetHandle);
        }

        private static class RadialBlurShaderParams
        {
            public static int center = Shader.PropertyToID("_Center");
            public static int intensity = Shader.PropertyToID("_Intensity");
            public static int delay = Shader.PropertyToID("_Delay");
            public static int sampleCount = Shader.PropertyToID("_SampleCount");
        }
    }
}
