using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPopUp : MonoBehaviour
{
    [SerializeField]
    GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player.Instance.gameObject)
        {
            ShowPop();
        }    
    }

    public void ShowPop()
    {
        PauseManager.Instance.PauseGame();
        PauseManager.Instance.UnavailableEsc();
        popup.SetActive(true);
    }

    public void ClosePop()
    {
        PauseManager.Instance.UnPauseGame();
        PauseManager.Instance.AvailableEsc();
        Destroy(gameObject);
    }
}
