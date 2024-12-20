using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public enum Axis
    {
        X, Y, Z
    }

    public Axis axis;
    public float rotationSpeed = 10f; // 회전 속도

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 선택한 축을 기준으로 회전
            switch (axis)
        {
            case Axis.X:
                rectTransform.Rotate(Vector3.right * rotationSpeed * Time.unscaledDeltaTime);
                break;
            case Axis.Y:
                rectTransform.Rotate(Vector3.up * rotationSpeed * Time.unscaledDeltaTime);
                break;
            case Axis.Z:
                rectTransform.Rotate(Vector3.forward * rotationSpeed * Time.unscaledDeltaTime);
                break;
        }
    }
}
