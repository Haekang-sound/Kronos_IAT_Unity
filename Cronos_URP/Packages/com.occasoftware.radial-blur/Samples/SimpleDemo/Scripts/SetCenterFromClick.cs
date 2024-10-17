using UnityEngine;

namespace OccaSoftware.RadialBlur.Demo
{
    public class SetCenterFromClick : MonoBehaviour
    {
        [SerializeField]
        SetRadialBlurExamples doBlur;
        bool isActive = false;

        void Update()
        {
            if (!isActive)
                return;

            if (Input.GetMouseButton(0))
            {
                doBlur.SetCenterFromScreenPoint(Input.mousePosition);
            }
        }

        public void SetState(bool value)
        {
            isActive = value;
        }
    }
}
