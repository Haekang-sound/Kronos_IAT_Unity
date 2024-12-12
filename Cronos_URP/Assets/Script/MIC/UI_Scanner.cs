using UnityEngine;


/// <summary>
/// 아틀라스 동상에 접근한 플레이어를 스캔하고
/// 상호작용 UI 띄우기 + 능력 개방 UI로 옮기는 클래스
/// </summary>
public class UI_Scanner : MonoBehaviour
{
    public GameObject player;
    public GameObject interText;
    public GameObject abilityUnlock;

	public GameObject cpUI;
	public GameObject skillUI;
	public GameObject keyGuide;

	private bool keyguid;
    private bool isPopup;
    private bool isInteracting;
    private SoundManager sm;

    private void Start()
    {
        player = Player.Instance.gameObject;
        interText = UIManager.Instance.Interactor;
        abilityUnlock = GameObject.Find("AbilityUnlock");

		cpUI = GameObject.Find("HUD_CP");
		skillUI = GameObject.Find("HUD_Skills");
		keyGuide = GameObject.Find("KeyGuide");

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
		if (isPopup && Input.GetKeyDown(KeyCode.F) && !isInteracting
            && !PauseManager.Instance.isPause)
		{
			PauseManager.Instance.UnavailableEsc();
            Interaction();
			cpUI.SetActive(false);
			skillUI.SetActive(false);
			keyguid = keyGuide.activeSelf;
			keyGuide.SetActive(false);
		}
		else if (isInteracting && Input.GetKeyDown(KeyCode.Escape) && !ScreenFader.Instance.faderCanvasGroup.gameObject.activeSelf)
		{
            TurnInteraction();
			cpUI.SetActive(true);
			skillUI.SetActive(true);
			keyGuide.SetActive(keyguid);
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
        // 1스테이지 목표 달성을 위해서
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
