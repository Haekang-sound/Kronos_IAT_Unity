using Cinemachine;
using UnityEngine;


/// <summary>
/// 옵션 창에서 카메라 감도를 조절하고 저장했을 때
/// 그 값을 로컬 저장소에 넣어놨다가 적용하는 클래스
/// 씨네머신 버주얼 카메라의 pov 값을 갖고 있는다
/// </summary>
public class POVpasser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineVirtualCamera virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachinePOV pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_VerticalAxis.m_MaxSpeed = (PlayerPrefs.GetInt("PlayerCam", 50) / 500f);
        pov.m_HorizontalAxis.m_MaxSpeed = (PlayerPrefs.GetInt("PlayerCamY", 80) / 500f);
        Debug.Log("저장된 값은 " + PlayerPrefs.GetInt("PlayerCam", 50) + " , "
            + PlayerPrefs.GetInt("PlayerCamY", 80));
    }

}
