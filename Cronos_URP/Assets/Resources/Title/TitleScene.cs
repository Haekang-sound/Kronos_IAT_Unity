using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MICScene");

        // ���� �ε����� ���� ��ȯ�ϴ� ����
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("���� ����!");
        Application.Quit();
    }
}
