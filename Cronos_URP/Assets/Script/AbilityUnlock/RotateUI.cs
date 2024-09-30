using UnityEngine;

public class RotateUI : MonoBehaviour
{
    public enum Axis
    {
        X, Y, Z
    }

    public bool active;
    public Axis axis;
    public float rotationSpeed = 10f; // ȸ�� �ӵ�

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (active)
        {
            // ������ ���� �������� ȸ��
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
}
