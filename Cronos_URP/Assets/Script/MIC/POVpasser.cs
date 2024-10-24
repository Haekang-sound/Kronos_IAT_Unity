using Cinemachine;
using UnityEngine;

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
