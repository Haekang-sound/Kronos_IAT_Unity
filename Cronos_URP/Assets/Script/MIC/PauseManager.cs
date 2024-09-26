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

    // ±×·± ½Ì±ÛÅÏÀ¸·Î ±¦ÂúÀº°¡
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
        //playerCam.gameObject.SetActive(true);
        Debug.Log("Unpause");
        playerInput?.SwitchCurrentActionMap("Player");
        if (player != null)
        {
            player.gameObject.GetComponent<InputReader>().enabled = true;
        }
        isPause = false;
        Time.timeScale = 1f;
    }
}
