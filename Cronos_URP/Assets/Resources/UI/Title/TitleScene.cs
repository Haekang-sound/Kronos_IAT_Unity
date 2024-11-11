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

    [SerializeField]
    GameObject optionPanel;

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
        optionPanel.SetActive(true);
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
