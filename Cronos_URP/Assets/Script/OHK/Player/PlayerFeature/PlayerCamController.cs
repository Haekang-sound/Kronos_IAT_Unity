using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerCamControler : MonoBehaviour
{
    // 싱글턴 객체 입니다. 
    public static PlayerCamControler Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            // 인스턴스가 없다면 계층 구조창에서 검색해서 가져옴.
            instance = FindObjectOfType<PlayerCamControler>();

            return instance;
        }
    }

    protected static PlayerCamControler instance;

    private CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera VirtualCamera { get { return _virtualCamera; } }

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _virtualCamera.Follow = Player.Instance.transform.Find("Orientation");
        _virtualCamera.LookAt = Player.Instance.transform.Find("Orientation");
    }
}
