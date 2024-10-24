using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
	[SerializeField]
	Transform target;
	[SerializeField]
	Camera mainCam;
	[SerializeField]
	GameObject targetUI;
	[SerializeField]
	Player player;

	AutoTargetting atTgt;
	bool isTgt;

	//public float uiScaler = 5.0f;

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
