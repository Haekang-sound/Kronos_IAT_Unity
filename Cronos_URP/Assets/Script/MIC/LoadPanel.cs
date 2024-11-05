using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// 어쩌다보니 로드/타이틀을 한 스크립트에서 하게 되었다
/// 이거맞나
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
