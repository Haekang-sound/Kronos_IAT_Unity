using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    //[SerializeField]
    //Image keyMouGuide;
    //[SerializeField]
    //Image padGuide;
    //[SerializeField]
    //Button keyMouButton;
    //[SerializeField]
    //Button padButton;
    //[SerializeField]
    //GameObject keyMouTitle;
    //[SerializeField]
    //GameObject padTitle;
    [SerializeField]
    PauseMenu pauseMenu;

    private void Start()
    {

    }

    void OnEnable()
    {
        //EventSystem.current.SetSelectedGameObject(keyMouButton.gameObject);
        //padGuide.gameObject.SetActive(false);
        //padTitle.gameObject.SetActive(false);
    }

    public void ShowKeyMou()
    {
        //padGuide.gameObject.SetActive(false);
        //keyMouGuide.gameObject.SetActive(true);
        //padTitle.gameObject.SetActive(false);
        //keyMouTitle.gameObject.SetActive(true);
    }

    public void ShowPad()
    {
        //padGuide.gameObject.SetActive(true);
        //keyMouGuide.gameObject.SetActive(false);
        //padTitle.gameObject.SetActive(true);
        //keyMouTitle.gameObject.SetActive(false);
    }

    public void ExitControl()
    {
        gameObject.SetActive(false);
        pauseMenu.isControl = false;
    }
}
