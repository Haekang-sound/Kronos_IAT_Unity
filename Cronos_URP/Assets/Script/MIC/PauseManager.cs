using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject playerCam;

    bool isPause = false;

    // �׷� �̱������� ��������
    private static PauseManager instance;
    public static PauseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PauseManager>();
                if (instance == null)
                {
                    GameObject pauseManager = new GameObject(typeof(PauseManager).Name);
                    instance = pauseManager.AddComponent<PauseManager>();

                    DontDestroyOnLoad(pauseManager);
                }
            }
            return instance;
        }
    }
    public static PlayerInput playerInput;

    public static bool isPaused { get; private set; }

    private void OnEnable()
    {
        player = Player.Instance;
        playerInput = GetComponent<PlayerInput>();
    }


    private void Update()
    {
        if (isPause)
            Time.timeScale = 0f;
    }

    public void PauseGame()
    {
		PlayerStateMachine.GetInstance().isPaused = true;
		GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = false;
        //playerCam.gameObject.SetActive(false);
        Debug.Log("Pause");

        playerInput?.SwitchCurrentActionMap("UI");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = false;
        }
        isPause = true;
    }

    public void UnPauseGame()
    {
        playerInput?.SwitchCurrentActionMap("Player");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = true;
        }
        isPause = false;
        Time.timeScale = 1f;
		GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = true;
		PlayerStateMachine.GetInstance().isPaused = false;
	}

    public void PausePlayer()
    {
        //PlayerStateMachine.GetInstance().isPaused = true;
        GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = false;

        playerInput?.SwitchCurrentActionMap("CutScene");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = false;
        }
    }

    public void UnPausePlayer()
    {
        playerInput?.SwitchCurrentActionMap("Player");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = true;
        }

        GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = true;
        //PlayerStateMachine.GetInstance().isPaused = false;
    }
}
