using UnityEngine;

public class UI_Scanner : MonoBehaviour
{
    public GameObject player;
    public GameObject interText;
    public GameObject abilityUnlock;

	public GameObject TPUI;
	public GameObject KeyGuid;

    bool isPopup;
    bool isInteracting;
    SoundManager sm;

    private void Start()
    {
        player = Player.Instance.gameObject;
        interText = UIManager.Instance.Interactor;
        abilityUnlock = GameObject.Find("AbilityUnlock");

		TPUI = GameObject.Find("HUD_TPCP");
		KeyGuid = GameObject.Find("KeyGuide");

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
			PauseManager.Instance.UnavailableEsc();
            Interaction();
			TPUI.SetActive(false);
			KeyGuid.SetActive(false);
		}
		else if (isInteracting && Input.GetKeyDown(KeyCode.Escape) && !ScreenFader.Instance.faderCanvasGroup.gameObject.activeSelf)
		{
            TurnInteraction();
			TPUI.SetActive(true);
			KeyGuid.SetActive(true);
            PauseManager.Instance.AvailableEsc();
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
        sm.PlaySFX("01 AbilityScreenFadeOut_Effect_Sound_SE", transform);
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
