using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.RadialBlur.Runtime
{
    [Serializable, VolumeComponentMenu("OccaSoftware/Radial Blur")]
    public sealed class RadialBlurPostProcess : VolumeComponent, IPostProcessComponent
    {
        public float GetIntensity() => intensity.GetValue<float>();

        public Vector2 GetCenter() => center.GetValue<Vector2>();

        public float GetDelay() => delay.GetValue<float>();

        public int GetSampleCount() => numberOfSamples.GetValue<int>();

        public void SetIntensity(float intensity)
        {
            this.intensity.overrideState = true;
            this.intensity.value = intensity;
        }

        public void SetSampleCount(int sampleCount)
        {
            this.numberOfSamples.overrideState = true;
            this.numberOfSamples.value = sampleCount;
        }

        public void SetDelay(float delay)
        {
            this.delay.overrideState = true;
            this.delay.value = delay;
        }

        public void SetCenter(Vector2 center)
        {
            this.center.overrideState = true;
            this.center.value = center;
        }

        /// <summary>
        /// Converts a given screenpoint to Radial Blur Center.
        /// Assumes screenpoint is measured in pixels, starting from [0,0] at the bottom left corner to [Screen.width, Screen.height] at the top right corner.
        /// </summary>
        /// <param name="screenPoint"></param>
        public void SetCenterFromScreenPoint(Vector2 screenPoint)
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 UV = screenPoint / screenSize;
            Vector2 UVHalfWidths = Vector2.one - (UV * new Vector2(2.0f, 2.0f));
            SetCenter(UVHalfWidths);
        }

        [SerializeField, Range(0f, 1f), Tooltip("Sets the amount to which the screen is blurred away from the center point. Default is 0.0.")]
        public MinFloatParameter intensity = new MinFloatParameter(0, 0);

        [Tooltip("Sets the Radial Blur Center Point. Measured in Screen Half-Widths. Screen center is [0.0, 0.0]. Default is [0.0, 0.0]")]
        public Vector2Parameter center = new Vector2Parameter(Vector2.zero);

        [Tooltip("The higher this value, the further out the blur will start. Default is 0.1")]
        public MinFloatParameter delay = new MinFloatParameter(0.1f, 0f);

        [Tooltip("Control the quality of the effect. Higher values are more computationally expensive but can provide smoother results.")]
        public ClampedIntParameter numberOfSamples = new ClampedIntParameter(16, 4, 64);

        public bool IsActive()
        {
            return intensity.value > 0;
        }

        public bool IsTileCompatible() => false;
    }
}
