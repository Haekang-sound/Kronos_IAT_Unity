using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// ���۹�. �ſ�ſ� �����ϰ� �������
public class ControlPanel : MonoBehaviour
{
    
    [SerializeField]
    PauseMenu pauseMenu;

    [SerializeField]
    int guideNum = 0;
    public int guideLength;

    public GameObject[] guides;

    private void Start()
    {
        guideLength = guides.Length - 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            CallGuide();
        }
    }

    public void ResetGuideNum()
    {
        guideNum = 0;
    }

    // ���� ���� ���̵带 �ѱ�� �Լ�
    public void CallGuide()
    {
        if (guideNum == guideLength)
        {
            if (transform.parent.GetComponent<KeyPopUp>() != null)
            {
                ExitInstance();
                return;
            }

            ExitControl();
            return;
        }

        for (int i = 0; i < guides.Length; i++)
        {
            guides[i].SetActive(false);
            guides[guideNum + 1].SetActive(true);
        }

        guideNum++;
    }

    void OnEnable()
    {
        foreach (GameObject g in guides)
        {
            g.SetActive(false);
        }

        guides[0].SetActive(true);
    }

    public void ShowKeyMou()
    {
        //padGuide.gameObject.SetActive(false);
        //keyMouGuide.gameObject.SetActive(true);
        //padTitle.gameObject.SetActive(false);
        //keyMouTitle.gameObject.SetActive(true);
    }

    public void ShowPad()
    {
        //padGuide.gameObject.SetActive(true);
        //keyMouGuide.gameObject.SetActive(false);
        //padTitle.gameObject.SetActive(true);
        //keyMouTitle.gameObject.SetActive(false);
    }

    public void ExitControl()
    {
        ResetGuideNum();
        gameObject.SetActive(false);
        pauseMenu.isControl = false;
    }

    public void ExitInstance()
    {
        PauseManager.Instance.UnPauseGame();
        PauseManager.Instance.AvailableEsc();
        Destroy(transform.parent.gameObject);
    }
}
