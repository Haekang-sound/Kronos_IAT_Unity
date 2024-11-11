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

    
    public bool isPause = false;
    // esc로 퍼즈할 수 있음을 허용하는 불리언
    public bool escAvailable;


    // 현재 능력개방 상태로 들어갔으니 tp를 멈추기 위한 불리언
    public bool abilityPause = false;

    // 그런 싱글턴으로 괜찮은가
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

    private void Start()
    {
        escAvailable = true;
    }

    private void Update()
    {
        if (isPause)
            Time.timeScale = 0f;
    }

    public void PauseGame()
    {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
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
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
        if (abilityPause)
            return;
        playerInput?.SwitchCurrentActionMap("Player");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = true;
        }
        isPause = false;
        Time.timeScale = 1f;
        Debug.Log("Unpause");
        GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = true;
		PlayerStateMachine.GetInstance().isPaused = false;
	}

    public void PausePlayer()
    {
        //PlayerStateMachine.GetInstance().isPaused = true;
		Player.Instance.isDecreaseTP = false;
        GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = false;

        playerInput?.SwitchCurrentActionMap("CutScene");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = false;
        }
    }

    public void UnPausePlayer()
    {
		Player.Instance.isDecreaseTP = true;
		playerInput?.SwitchCurrentActionMap("Player");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = true;
        }

        GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = true;
        //PlayerStateMachine.GetInstance().isPaused = false;
    }

    public void AvailableEsc()
    {
        escAvailable = true;
    }

    public void UnavailableEsc()
    {
        escAvailable = false;
    }
}
