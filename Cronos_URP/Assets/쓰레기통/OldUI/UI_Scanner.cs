using UnityEngine;

public class UI_Scanner : MonoBehaviour
{
    public GameObject player;
    public GameObject interText;
    bool isPopup;
    bool isInteracting;


    private void Start()
    {
        player = Player.Instance.gameObject;
        interText = GameObject.Find("UI_Interact");
        interText.SetActive(false);
        isPopup = false;
        isInteracting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger");
            interText.SetActive(true);
            isPopup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Trigger Exit");
            interText.SetActive(false);
            isPopup = false;
        }
    }

    public void ExitInteracting()
    {
        isInteracting = false;
        Debug.Log("Exit InterAction");
    }

    void Update()
    {
        if (isPopup && Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            
        }
    }

}
