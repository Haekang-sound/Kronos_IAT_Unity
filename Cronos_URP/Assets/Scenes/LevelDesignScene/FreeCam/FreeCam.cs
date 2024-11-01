using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class moveObject : MonoBehaviour
{

    private float speed_move = 3.0f;  // �⺻ �̵� �ӵ�
    private float rotationSpeed = 2.0f;  // ȸ�� �ӵ�
    private float verticalSpeed = 3.0f;  // Q�� E Ű�� �̵��� ���� �ӵ�
    private float zoomSpeed = 10f;  // ȭ�� ���� �ӵ�
    private float minFieldOfView = 15f;  // �ּ� ȭ��
    private float maxFieldOfView = 90f;  // �ִ� ȭ��
    private float pitch = 0f;  // ���� ȸ�� ���� (X�� ȸ��)
    private float yaw = 0f;  // ���� ȸ�� ���� (Y�� ȸ��)
    private float speedAdjustment = 0.5f;  // �̵� �ӵ� ���� ����

    private bool showSpeed = false;  // �ӵ� ǥ�� ����
    private float displaySpeedTime = 2.0f;  // �ӵ� ǥ�� ���� �ð�
    private float speedDisplayTimer;  // Ÿ�̸�

    // �� �̸��� string���� �����Ͽ� �ν����Ϳ� �Է� �����ϰ� ��
    public string sceneF1; // F1 Ű�� ������ �� �̸�
    public string sceneF2; // F2 Ű�� ������ �� �̸�
    public string sceneF3; // F3 Ű�� ������ �� �̸�
    public string sceneF4; // F4 Ű�� ������ �� �̸�

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // �ʱ⿡�� ���콺 ������ ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveObjectFunc();

        // 4�� Ű�� ������ ���콺 ������ ���̱�
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleCursorVisibility();
        }

        // + Ű�� �̵� �ӵ� ����
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            speed_move += speedAdjustment;
            ShowSpeed();
        }

        // - Ű�� �̵� �ӵ� ����
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            speed_move = Mathf.Max(0.1f, speed_move - speedAdjustment); // �ӵ��� 0 ���Ϸ� �������� �ʵ��� ����
            ShowSpeed();
        }

        // �ӵ� ǥ�� Ÿ�̸� ������Ʈ
        if (showSpeed)
        {
            speedDisplayTimer -= Time.deltaTime;
            if (speedDisplayTimer <= 0)
            {
                showSpeed = false;
            }
        }

        // F1 ~ F4 Ű�� �� ��ȯ ��� �߰�
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
        // �⺻ �̵�
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * speed_move * Time.deltaTime;
        keyV = keyV * speed_move * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.forward * keyV);

        // Q�� E�� ���� �̵�
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }

        // ���콺 ������ ��ư�� ���� ���¿��� ����, ���� ȸ��
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);  // ���� ȸ�� ���� ����

            transform.eulerAngles = new Vector3(pitch, yaw, 0f);  // Z�� ȸ�� ����
        }

        // ���콺 �ٷ� ȭ�� ����
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

    // ���콺 ������ ����/ǥ�� ��ȯ
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

    // �ӵ� ǥ�� Ȱ��ȭ �Լ�
    void ShowSpeed()
    {
        showSpeed = true;
        speedDisplayTimer = displaySpeedTime;
    }

    // �ӵ� ǥ�� GUI
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