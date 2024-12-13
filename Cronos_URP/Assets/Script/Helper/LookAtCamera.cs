using UnityEngine;

/// <summary>
/// 카메라를 기준으로 오브젝트의 회전 방식을 설정하는 클래스입니다.
/// 주어진 모드에 따라 카메라를 바라보거나 카메라의 방향을 반영하여 회전합니다.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt, // 카메라를 바라보도록 회전
        LookAtInverted, // 카메라를 반대로 바라보도록 회전
        CameraForward, // 카메라의 전방 방향으로 오브젝트의 Z축을 맞추기
        CameraForwardInverted, // 카메라의 전방 방향으로 오브젝트의 Z축을 반대로 맞추기
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
                //* 카메라 방향을 알아내서 그 방향 만큼 돌려줘서 반전시키기
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                //* 카메라 방향으로 Z축 (앞뒤)을 바꿔주기
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                //* 카메라 방향으로 Z축 (앞뒤)을 바꿔주고 반전시키기
                transform.forward = -Camera.main.transform.forward;
                break;
            default:

                break;
        }
    }
}
