using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 조작법 패널 클래스
/// 단순하게 인덱스에 따라 배열에 맞는 게임오브젝트를 띄우는 방식
/// </summary>
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

    // 인덱스 0부터 시작해서 다음으로 넘긴다
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

    // 패널 끄면 다시 인덱스 0으로
    public void ExitControl()
    {
        ResetGuideNum();
        gameObject.SetActive(false);
        pauseMenu.isControl = false;
    }

    // 퍼즈 메뉴에서 온게 아니라 스테이지1에서 띄워줬다면
    public void ExitInstance()
    {
        PauseManager.Instance.UnPauseGame();
        PauseManager.Instance.AvailableEsc();
        Destroy(transform.parent.gameObject);
    }
}
