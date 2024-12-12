using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 퍼즈 했을 때 등장하는 메뉴 UI
/// 프리팹에서 관리하기 쉽게 다 인스펙터로 열어놓은 게임오브젝트 뭉치
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private PauseManager pauseManager;

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject optionPanel;
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject loadPanel;
    [SerializeField]
    private GameObject titlePanel;

    private bool isPaused;
    public bool isOption;
    public bool isControl;
    public bool isLoad;
    public bool isTitle;

    private void OnEnable()
    {
        pauseManager = PauseManager.Instance;
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        loadPanel.SetActive(false);
        titlePanel.SetActive(false);
        isPaused = false;
    }

    public void OpenOption()
    {
        optionPanel.SetActive(true);
        isOption = true;
    }

    public void OpenControl()
    {
        controlPanel.SetActive(true);
        isControl = true;
    }

    public void OpenCheckPoint()
    {
        loadPanel.SetActive(true);
        isLoad = true;
    }

    public void ExitTitle()
    {
        titlePanel.SetActive(true);
        isTitle = true;
    }

    public void Close()
    {
        pausePanel.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseManager.escAvailable && !ScreenFader.Instance.faderCanvasGroup.gameObject.activeSelf)
        {
            if (!isPaused)
            {
                OpenPauseMenu();
            }

            else
            {
                ClosePanel();
            }
        }
    }

    void OpenPauseMenu()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        pauseManager.PauseGame();
        Debug.Log("퍼즈메뉴열기");
    }

    void ClosePauseMenu()
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        isPaused = false;
        pauseManager.UnPauseGame();
        Debug.Log("퍼즈메뉴닫기");
    }

    public void ClosePanel()
    {
        if (isOption)
        {
            optionPanel.GetComponentInChildren<SoundMixerNCamera>().NoButton();
            optionPanel.GetComponentInChildren<SoundMixerNCamera>().ExitPanel();
            isOption = false;
        }

        else if (isControl)
        {
            controlPanel.SetActive(false);
            isControl = false;
        }

        else if (isLoad)
        {
            loadPanel.GetComponent<LoadPanel>().ExitLoad();
            isLoad = false;
        }

        else if (isTitle)
        {
            titlePanel.GetComponent<LoadPanel>().ExitLoad();
            isTitle = false;
        }

        else
        {
            ClosePauseMenu();
        }

        // 버튼 하이라이트 초기화
        Debug.Log("버튼 하이라이트 초기화");
        EventSystem.current.SetSelectedGameObject(null);
    }
}
