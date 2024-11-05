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
		if (isPopup && Input.GetKeyDown(KeyCode.F) && !isInteracting)
		{
            Interaction();
		}
		else if (isInteracting && Input.GetKeyDown(KeyCode.F))
		{
            TurnInteraction();
		}
	}

    void Interaction()
    {
        isInteracting = true;
        sm.PlaySFX("00 AbilityScreen_FadeIN_Sound_SE", transform);
        interText.SetActive(false);
        PauseManager.Instance.abilityPause = true;
        abilityUnlock.GetComponent<AbilityTree>().EnterAbility();
    }

    void TurnInteraction()
    {
        isInteracting = false;
        abilityUnlock.GetComponent<AbilityTree>().ExitAbility();
        Invoke("ShowIntext", 0.3f);
        isPopup = true;
        // 크로노스 네 이노오ㅗㅁ
        if (QuestManager.Instance.abilityQuesting)
        {
            QuestManager.Instance.abilityQuesting = false;
            Invoke("JustAchieve", 0.3f);
        }
    }

    void ShowIntext()
    {
        interText.SetActive(true);
    }

    void JustAchieve()
    {
        UIManager.Instance.Achieve();
    }
}
