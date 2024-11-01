using UnityEngine;

public class UI_Scanner : MonoBehaviour
{
    public GameObject player;
    public GameObject interText;
    public GameObject abilityUnlock;
    bool isPopup;
    bool isInteracting;
    SoundManager sm;

    private void Start()
    {
        player = Player.Instance.gameObject;
        interText = UIManager.Instance.Interactor;
        abilityUnlock = GameObject.Find("AbilityUnlock");
        interText.SetActive(false);
        isPopup = false;
        isInteracting = false;
        sm = SoundManager.Instance;
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
        if (isPopup && Input.GetKeyDown(KeyCode.Tab) && !isInteracting)
        {
            isInteracting = true;
            sm.PlaySFX("00 AbilityScreen_FadeIN_Sound_SE", transform);
            interText.SetActive(false);
            abilityUnlock.GetComponent<AbilityTree>().EnterAbility();
        }

        if (isInteracting && Input.GetKeyDown(KeyCode.Tab))
        {
            isInteracting = false;
            abilityUnlock.GetComponent<AbilityTree>().ExitAbility();
            interText.SetActive(true);
            isPopup = true;
            // 크로노스 네이놈
            if (QuestManager.Instance.abilityQuesting)
            {
                QuestManager.Instance.abilityQuesting = false;
                UIManager.Instance.Achieve();
            }
        }
    }

}
