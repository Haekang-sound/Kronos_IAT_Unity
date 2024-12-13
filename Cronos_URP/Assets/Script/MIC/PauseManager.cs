using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// 게임을 일시정지하는 역할을 맡은 클래스
/// 기본적으로 esc로 타임스케일을 멈춘다
/// 일시정지에서 일어나면 안되는 것들은 막는다
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject playerCam;

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

    // 퍼즈했을 때 신경써야 하는 것들
    public void PauseGame()
    {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PlayerStateMachine.GetInstance().isPaused = true;
		GameObject.Find("PlayerCam").GetComponent<CinemachineInputProvider>().enabled = false;
        Debug.Log("Pause");

        // 플레이어 인풋을 차단한다
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
