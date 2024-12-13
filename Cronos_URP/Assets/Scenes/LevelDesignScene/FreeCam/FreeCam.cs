using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class moveObject : MonoBehaviour
{

    private float speed_move = 3.0f;  // 기본 이동 속도
    private float rotationSpeed = 2.0f;  // 회전 속도
    private float verticalSpeed = 3.0f;  // Q와 E 키로 이동할 때의 속도
    private float zoomSpeed = 10f;  // 화각 조절 속도
    private float minFieldOfView = 15f;  // 최소 화각
    private float maxFieldOfView = 90f;  // 최대 화각
    private float pitch = 0f;  // 수직 회전 각도 (X축 회전)
    private float yaw = 0f;  // 수평 회전 각도 (Y축 회전)
    private float speedAdjustment = 0.5f;  // 이동 속도 조절 간격

    private bool showSpeed = false;  // 속도 표시 여부
    private float displaySpeedTime = 2.0f;  // 속도 표시 지속 시간
    private float speedDisplayTimer;  // 타이머

    // 씬 이름을 string으로 선언하여 인스펙터에 입력 가능하게 함
    public string sceneF1; // F1 키에 연결할 씬 이름
    public string sceneF2; // F2 키에 연결할 씬 이름
    public string sceneF3; // F3 키에 연결할 씬 이름
    public string sceneF4; // F4 키에 연결할 씬 이름

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // 초기에는 마우스 포인터 숨김
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveObjectFunc();

        // 4번 키를 누르면 마우스 포인터 보이기
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleCursorVisibility();
        }

        // + 키로 이동 속도 증가
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            speed_move += speedAdjustment;
            ShowSpeed();
        }

        // - 키로 이동 속도 감소
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            speed_move = Mathf.Max(0.1f, speed_move - speedAdjustment); // 속도가 0 이하로 떨어지지 않도록 설정
            ShowSpeed();
        }

        // 속도 표시 타이머 업데이트
        if (showSpeed)
        {
            speedDisplayTimer -= Time.deltaTime;
            if (speedDisplayTimer <= 0)
            {
                showSpeed = false;
            }
        }

        // F1 ~ F4 키에 씬 전환 기능 추가
        if (Input.GetKeyDown(KeyCode.F1) && !string.IsNullOrEmpty(sceneF1))
        {
            SceneManager.LoadScene(sceneF1);
        }
        if (Input.GetKeyDown(KeyCode.F2) && !string.IsNullOrEmpty(sceneF2))
        {
            SceneManager.LoadScene(sceneF2);
        }
        if (Input.GetKeyDown(KeyCode.F3) && !string.IsNullOrEmpty(sceneF3))
        {
            SceneManager.LoadScene(sceneF3);
        }
        if (Input.GetKeyDown(KeyCode.F4) && !string.IsNullOrEmpty(sceneF4))
        {
            SceneManager.LoadScene(sceneF4);
        }
    }

    void moveObjectFunc()
    {
        // 기본 이동
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * speed_move * Time.deltaTime;
        keyV = keyV * speed_move * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.forward * keyV);

        // Q와 E로 수직 이동
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }

        // 마우스 오른쪽 버튼이 눌린 상태에서 수평, 수직 회전
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);  // 수직 회전 각도 제한

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);  // Z축 회전 고정
        }

        // 마우스 휠로 화각 조절
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            Camera camera = GetComponent<Camera>();
            if (camera != null)
            {
                camera.fieldOfView -= scrollInput * zoomSpeed;
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFieldOfView, maxFieldOfView);
            }
        }
    }

    // 마우스 포인터 숨김/표시 전환
    void ToggleCursorVisibility()
    {
        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // 속도 표시 활성화 함수
    void ShowSpeed()
    {
        showSpeed = true;
        speedDisplayTimer = displaySpeedTime;
    }

    // 속도 표시 GUI
    void OnGUI()
    {
        if (showSpeed)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 200, 50), "Speed: " + speed_move.ToString("F1"), style);
        }
    }
}