using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Camera playerCam;
    [SerializeField]
    GameObject targetUI;
    [SerializeField]
    Player player;

    AutoTargetting atTgt;
    bool isTgt;

    //public float uiScaler = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        atTgt = AutoTargetting.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        target = atTgt.GetTarget();
        isTgt = player.IsLockOn;
    }

    private void LateUpdate()
    {
        if (target && isTgt)
        {
            targetUI.SetActive(true);

            Vector3 dir = (target.transform.position - player.transform.position).normalized;

            targetUI.transform.position = target.position - new Vector3(dir.x, 0, dir.z);
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            targetUI.SetActive(false);
        }

        
    }

    //ui 오버레이로 설정할 경우
    //targetUI.transform.position = playerCam.WorldToScreenPoint(target.position) + new Vector3(0, yUp, 0);
    //targetUI.transform.localScale = new Vector3(uiScaler / targetUI.transform.position.z, uiScaler / targetUI.transform.position.z, 0f);

    
}
