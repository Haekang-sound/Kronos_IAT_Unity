using UnityEngine;

/// <summary>
/// ī�޶� �������� ������Ʈ�� ȸ�� ����� �����ϴ� Ŭ�����Դϴ�.
/// �־��� ��忡 ���� ī�޶� �ٶ󺸰ų� ī�޶��� ������ �ݿ��Ͽ� ȸ���մϴ�.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt, // ī�޶� �ٶ󺸵��� ȸ��
        LookAtInverted, // ī�޶� �ݴ�� �ٶ󺸵��� ȸ��
        CameraForward, // ī�޶��� ���� �������� ������Ʈ�� Z���� ���߱�
        CameraForwardInverted, // ī�޶��� ���� �������� ������Ʈ�� Z���� �ݴ�� ���߱�
    }

    [SerializeField] private Mode _mode;
    private void LateUpdate()
    {
        switch (_mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                //* ī�޶� ������ �˾Ƴ��� �� ���� ��ŭ �����༭ ������Ű��
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                //* ī�޶� �������� Z�� (�յ�)�� �ٲ��ֱ�
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                //* ī�޶� �������� Z�� (�յ�)�� �ٲ��ְ� ������Ű��
                transform.forward = -Camera.main.transform.forward;
                break;
            default:

                break;
        }
    }
}
