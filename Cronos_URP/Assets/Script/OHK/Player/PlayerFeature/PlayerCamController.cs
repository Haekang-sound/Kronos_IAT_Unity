using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerCamControler : MonoBehaviour
{
    // �̱��� ��ü �Դϴ�. 
    public static PlayerCamControler Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            // �ν��Ͻ��� ���ٸ� ���� ����â���� �˻��ؼ� ������.
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
