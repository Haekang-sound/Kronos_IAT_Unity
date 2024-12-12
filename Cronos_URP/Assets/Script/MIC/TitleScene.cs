using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// 타이틀 씬에서 가지고 있는 클래스
/// 시작 씬, 크레딧 씬, 게임 오버만 지원한다
/// </summary>
public class TitleScene : MonoBehaviour
{
    [SerializeField]
    private Button sButton;
    [SerializeField]
    private Button cButton;
    [SerializeField]
    private Button eButton;

    public void StartGame()
    {
		SceneManager.LoadScene("1_HN_Scene2");
	}

    public void Credit()
    {
		SceneManager.LoadScene("5_Ending");
	}

    public void ExitGame()
    {
        Debug.Log("께임 종료!");
        Application.Quit();
    }
}
