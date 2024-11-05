using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// ��¼�ٺ��� �ε�/Ÿ��Ʋ�� �� ��ũ��Ʈ���� �ϰ� �Ǿ���
/// �̰Ÿ³�
public class LoadPanel : MonoBehaviour
{
    [SerializeField]
    Button option;
    [SerializeField]
    Button control;
    [SerializeField]
    Button load;
    [SerializeField]
    Button title;
    [SerializeField]
    PauseMenu pauseMenu;

    private void OnEnable()
    {
        option.gameObject.SetActive(false);
        control.gameObject.SetActive(false);
        load.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
    }

    public void LoadSave()
    {

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
