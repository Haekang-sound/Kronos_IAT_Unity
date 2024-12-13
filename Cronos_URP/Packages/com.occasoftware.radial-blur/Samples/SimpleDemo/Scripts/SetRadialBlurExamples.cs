using UnityEngine;
using OccaSoftware.RadialBlur.Runtime;
using UnityEngine.Rendering;

namespace OccaSoftware.RadialBlur.Demo
{
    public class SetRadialBlurExamples : MonoBehaviour
    {
        RadialBlurPostProcess radialBlur = null;
        public Volume volume;
        private VolumeProfile profile = null;

        private void Start()
        {
            ValidateRadialBlurManager();
        }

        private bool ValidateRadialBlurManager()
        {
            profile = volume.sharedProfile;
            return profile.TryGet(out radialBlur);
        }

        public void SetDelay(float delay)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetDelay(delay);
        }

        public void SetIntensity(float value)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetIntensity(value);
        }

        public void SetCenter(Vector2 value)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetCenter(value);
        }

        public void ResetCenter()
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetCenter(Vector2.zero);
        }

        public void SetCenterFromScreenPoint(Vector2 value)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetCenterFromScreenPoint(value);
        }

        public void SetSamples(int value)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetSampleCount(value);
        }

        public void SetSamplesFromFloat(float value)
        {
            if (ValidateRadialBlurManager())
                radialBlur.SetSampleCount((int)value);
        }
    }
}
