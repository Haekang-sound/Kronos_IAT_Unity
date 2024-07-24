using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    // 내가침
    [Header("References")]

    public Transform orientation;   // 방향
    public Transform player;        // 플레이어 최상위 오브젝트
    public Transform playerObj;     // 모델, 충돌
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    Vector3 viewDir;
    Vector3 dirToCombatLookAt;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    void Start()
    {
     
        SwitchCameraSytle(currentStyle);
    }

    void Update()
    {
        // switch sytles 
        if (Input.GetKeyDown(KeyCode.Alpha1))       // 1번을 누르면
        {
            SwitchCameraSytle(CameraStyle.Basic);   // 카메라가 baisc으로
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCameraSytle(CameraStyle.Combat);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCameraSytle(CameraStyle.Topdown);
        }

        //rotate orientation(방향)
        // 시점은 카메라입장에서 바라보는 플레이어 방향이다.
        viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized; // 캐릭터의 방향은 시점과 같다

        // rotate player object(모델 회전)
        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // 입력을 토대로 방향을 결정한다.
            // 현제 캐릭터 전방 * 방향(+,_) + 현재 캐릭터 좌우 * 방향(+,-)
            // 을 이용해서 방향을 입력받는다
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (inputDir != Vector3.zero)
            {
                // 인풋방향 벡터가 0이 아니라면(입력을 받았을 경우)
                // 플레이어의 전방벡터는 기존 전방벡터에서 인풋방향벡터방향으로 회전속도만큼 구형선형보간한 값으로 움직인다.
                player.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }

        // 카메라상태가 Combat상태일 경우
        else if (currentStyle == CameraStyle.Combat)
        {
            // 전투시점 벡터는 카메라에서 combatLookAt오브젝트를 바라본 방향이다.
            dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            // 플레이어의 방향은 combatlookat의 방향과 같다.
            orientation.forward = dirToCombatLookAt.normalized;
            // 모델의 전방벡터도 combatlookat의 방향과 같다.
            playerObj.forward = dirToCombatLookAt.normalized;
            //player.forward = dirToCombatLookAt.normalized;

        }
    }

    // 카메라 스타일 바꾸기
    void SwitchCameraSytle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic)
        {
            thirdPersonCam.GetComponent<Transform>().position = Camera.main.transform.position;
            orientation.forward = dirToCombatLookAt.normalized;
            playerObj.forward = dirToCombatLookAt.normalized;
            player.forward = dirToCombatLookAt.normalized;
            thirdPersonCam.SetActive(true);
        }
        if (newStyle == CameraStyle.Combat)
        {
            combatCam.SetActive(true);
        }
        if (newStyle == CameraStyle.Topdown)
        {
            topDownCam.SetActive(true);
        }

        currentStyle = newStyle;
    }
}
