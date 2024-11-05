using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    PauseManager pauseManager;

    [SerializeField]
    GameObject pausePanel;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject controlPanel;
    [SerializeField]
    GameObject loadPanel;
    [SerializeField]
    GameObject titlePanel;
    bool isPaused;
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
        if (Input.GetKeyDown(KeyCode.Escape) && pauseManager.escAvailable)
        {
            if (!isPaused)
                OpenPauseMenu();

            else
                ClosePanel();
        }
    }

    void OpenPauseMenu()
    {
        pausePanel.SetActive(true);
        isPaused = true;
        pauseManager.PauseGame();
        Debug.Log("����޴�����");
    }

    void ClosePauseMenu()
    {
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        isPaused = false;
        pauseManager.UnPauseGame();
        Debug.Log("����޴��ݱ�");
    }

    void ClosePanel()
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
        // ��ư ���̶���Ʈ �ʱ�ȭ
        Debug.Log("��ư ���̶���Ʈ �ʱ�ȭ");
        EventSystem.current.SetSelectedGameObject(null);
    }
}
