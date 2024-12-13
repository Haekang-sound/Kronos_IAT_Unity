using UnityEngine;


/// <summary>
/// 첫 번째 스테이지에서 박스 콜라이더에 닿으면
/// 조작법 가이드를 띄워주는 클래스
/// </summary>
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
