using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    [SerializeField]
    Button sButton;
    [SerializeField]
    Button oButton;
    [SerializeField]
    Button cButton;
    [SerializeField]
    Button eButton;

    void Update()
    {

    }

    public void StartGame()
    {
		SceneManager.LoadScene("1_HN_Scene2");
	}

	public void Title()
	{
		SceneManager.LoadScene("0_TitleTest");
	}

    public void Option()
    {
		SceneManager.LoadScene("3_EJ_Scene2");
	}

    public void Credit()
    {

    }

    public void ExitGame()
    {
        Debug.Log("께임 종료!");
        Application.Quit();
    }


}
