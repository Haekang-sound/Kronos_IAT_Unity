using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void StartGame()
    {
        //SceneManager.LoadScene("MICScene");
        //SceneManager.LoadScene("OHK_Scene");
		GameManager.Instance.SwitchScene("OHK_Scene");
		//GameManager.Instance.SceneTransition("OHK_Scene");

        // ���� �ε����� ���� ��ȯ�ϴ� ����
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("���� ����!");
        Application.Quit();
    }
}
