using UnityEngine;


/// <summary>
/// 락온 타겟 UI를 보여주는 클래스
/// UI가 플레이어와 타겟 사이에 위치해야 하므로 월드 공간에 띄워준다
/// 오버레이로 할 수도 있다
/// </summary>
public class LockOnTarget : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	[SerializeField]
	private Camera mainCam;
	[SerializeField]
	private GameObject targetUI;
	[SerializeField]
	private Player player;

	private AutoTargetting atTgt;
	private bool isTgt;

	// Start is called before the first frame update
	void Start()
	{
		atTgt = AutoTargetting.GetInstance();
		mainCam = Camera.main;
		targetUI = transform.GetChild(0).gameObject;
		player = Player.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (atTgt && atTgt.GetTarget() != null)
		{
			target = atTgt.GetTarget();
		}
		isTgt = player.IsLockOn;
	}

	// 플레이어나 타겟의 위치는 Update에서 변경되므로 그 다음에
	private void LateUpdate()
	{
		if (target && isTgt)
		{
			targetUI.SetActive(true);

			Vector3 dir = (target.transform.position - player.transform.position).normalized;
			transform.position = target.transform.position;
			targetUI.transform.position = target.position - new Vector3(dir.x, 0, dir.z);
			transform.forward = Camera.main.transform.forward;
		}
		else
		{
			targetUI.SetActive(false);
		}

	}

	//ui 오버레이로 설정할 경우
	//targetUI.transform.position = playerCam.WorldToScreenPoint(target.position) + new Vector3(0, yUp, 0);
	//targetUI.transform.localScale = new Vector3(uiScaler / targetUI.transform.position.z, uiScaler / targetUI.transform.position.z, 0f);


}
