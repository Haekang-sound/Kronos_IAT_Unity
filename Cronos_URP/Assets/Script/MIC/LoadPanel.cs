using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 체크포인트 로드와 타이틀 화면으로 돌아가는 기능을 하는 클래스
/// 체크포인트 버튼과 타이틀 버튼은 같은 스크립트를 사용한다
/// 굳이 나눌 필요가 없어 보여서 하나의 스크립트로 짜보았다
/// </summary>
public class LoadPanel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("퍼즈 패널의 옵션 버튼입니다")]
    private Button option;
    [SerializeField]
    [Tooltip("퍼즈 패널의 조작 버튼입니다")]
    private Button control;
    [SerializeField]
    [Tooltip("퍼즈 패널의 체크포인트 버튼입니다")]
    private Button load;
    [SerializeField]
    [Tooltip("퍼즈 패널의 타이틀 버튼입니다")]
    private Button title;
    [SerializeField]
    [Tooltip("상위 퍼즈메뉴 프리팹입니다")]
    private PauseMenu pauseMenu;

    private void OnEnable()
    {
        option.gameObject.SetActive(false);
        control.gameObject.SetActive(false);
        load.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
    }

    public void LoadSave()
    {
        SaveLoadManager.Instance.LoadCheckpointData();
    }

    public void GoTitle()
    {
		SceneManager.LoadScene("0_TitleTest");
    }

    public void ExitLoad()
    {
        gameObject.SetActive(false);
        option.gameObject.SetActive(true);
        control.gameObject.SetActive(true);
        load.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        pauseMenu.isLoad = false;
        pauseMenu.isTitle = false;
    }
}
