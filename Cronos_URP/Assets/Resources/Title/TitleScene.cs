using UnityEngine;
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
        //SceneManager.LoadScene("MICScene");
        //SceneManager.LoadScene("OHK_Scene");
		GameManager.Instance.SwitchScene("OHK_Scene");
		//GameManager.Instance.SceneTransition("OHK_Scene");

        // ���� �ε����� ���� ��ȯ�ϴ� ����
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Option()
    {

    }

    public void Credit()
    {

    }

    public void ExitGame()
    {
        Debug.Log("���� ����!");
        Application.Quit();
    }


}
