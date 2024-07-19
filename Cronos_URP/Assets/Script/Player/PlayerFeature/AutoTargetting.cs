using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
public class AutoTargetting : MonoBehaviour
{
	static AutoTargetting instance;
	public static AutoTargetting GetInstance() { return instance; }

	public CinemachineVirtualCamera PlayerCamera;
	public CinemachinePOV CinemachinePOV;

	public float horizontalSpeed = 10.0f; // 수평 회전 속도
	public float verticalSpeed = 3f;    // 수직 회전 속도

	public GameObject Player;       // 플레이어
	public Transform Target = null; // Player가 바라볼 대상
	public Transform PlayerObject;	// 플레이어 오브젝트 

	Transform maincamTransform;

	public float lockOnAixsDamp = 0.99f;			// 어느정도까지 따라갈 것인가!
	public float autoTargettingAixsDamp = 0.99f;	// 어느정도까지 따라갈 것인가!
	public float exitValue;

	private PlayerStateMachine stateMachine;

	private Vector3 direction;
	private float xDotResult;
	private float yDotResult;
	private bool isTargetting = false;
	private bool isFacing = false;


	// 몬스터리스트
	List<GameObject> MonsterList;
	float? min = null;

	private void Awake()
	{
		instance = this;	// 싱글턴으로 쓰겠다.
		MonsterList = new List<GameObject>(); // 몬스터를 담자
	}

	public Transform GetTarget()
	{
		return Target;
	}

	// Start is called before the first frame update
	public void OnEnable()
	{
		stateMachine = Player.GetComponent<PlayerStateMachine>();

		maincamTransform = Camera.main.transform;
		CinemachinePOV = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
		var pov = PlayerCamera.GetCinemachineComponent<CinemachinePOV>();
		var test = PlayerCamera.IsValid;
	}

	/// 콜라이더에 몬스터가 들어오면 리스트에 추가한다.
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Add(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}

	}
	/// 콜라이더에서 몬스터가 나가면 리스트에서 제거한다.
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			MonsterList.Remove(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Player가 바라볼 방향을 정한다.
		if (Target != null)
		{
			direction = Target.position - PlayerObject.position;
			direction.y = 0;
		}

		xDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.right, Vector3.Cross(Vector3.up, direction.normalized).normalized)); 
		yDotResult = Mathf.Abs(Vector3.Dot(maincamTransform.up, Vector3.Cross(Vector3.right, direction.normalized).normalized));

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (FindTarget())
			{
				isTargetting = true;
				StartCoroutine(AutoTarget());
			}
			else
			{
				return;
			}
		}

		if (stateMachine.Player.IsLockOn)
		{
			LockOn();
		}  
	}

	private void FixedUpdate()
	{
		// Player가 몬스터 방향으로 몸을 돌린다.
		if ((isTargetting || stateMachine.Player.IsLockOn || isFacing) && stateMachine.GetState().ToString() != "PlayerParryState")
		{
			stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(direction.normalized), 0.1f));
		}
	}

	/// <summary>
	/// 오브젝트를 순회하며 찾는다.
	/// </summary>
	/// <param name="parent">부모 오브젝트</param>
	/// <param name="childName">자식 오브젝트 이름</param>
	/// <returns></returns>
	Transform FindChildRecursive(Transform parent, string childName)
	{
		// 현재 부모의 하위 오브젝트들을 순회합니다.
		foreach (Transform child in parent)
		{
			if (child.name == childName)
			{
				return child;
			}

			// 재귀적으로 하위 오브젝트들을 탐색합니다.
			Transform found = FindChildRecursive(child, childName);
			if (found != null)
			{
				return found;
			}
		}

		// 이름에 맞는 하위 오브젝트를 찾지 못하면 null을 반환합니다.
		return null;
	}

	// 타겟을 바라보는 건 언제 끝나지? 
	private void FacingTarget()
	{
		if (Mathf.Abs(Vector3.Dot(stateMachine.transform.forward, direction.normalized)) > 0.9f)
		{
			isFacing = false;
		}
	}

	private void StopAutoTargetting()
	{
		isTargetting = false;
	}

	private IEnumerator AutoTarget()
	{
		isTargetting = true;
		isFacing = true;

		while (isFacing)
		{
			FacingTarget();
			yield return null;
		}

		while (isTargetting)
		{
			if (Target == null)
			{
				StopAutoTargetting();
			}
			else
			{
				Vector3 targetPos = TransformPosition(maincamTransform, Target.position);

				// 내적값이 0.99 보다 작으면 더한다.
				if (xDotResult < autoTargettingAixsDamp)
				{	
					if (xDotResult > exitValue && stateMachine.InputReader.moveComposite.magnitude != 0f)
					{
						StopAutoTargetting();
					}
					TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - xDotResult));
				}
				else
				{
					StopAutoTargetting();
				}
			}
			yield return null;
		}

	}

	// 카메라를 돌린다
	private void TurnCamHorizontal(float value)
	{
		CinemachinePOV.m_HorizontalAxis.Value += value;
	}
	private void TurnCamVertical(float value)
	{
		CinemachinePOV.m_VerticalAxis.Value -= value;
	}
	// 특정 포지션을 특정 트렌스폼에서 바라본다.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}
	public bool FindTarget()
	{
		// 몬스터리스트가 없거나 사이즈가 0이라면 false
		if (MonsterList == null || MonsterList.Count == 0)
		{
			Target = null;
			return false;
		}
		if(stateMachine.Player.IsLockOn)
		{
			return true;
		}

		// 현재 몬스터 목록을 순회한다.
		for (int i = MonsterList.Count - 1; i >= 0; i--)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.RemoveAt(i);
			}
		}

		// 몬스터리스트를 거리 순으로 정렬한다.
		SortMosterList();

		// 현재 몬스터 목록을 순회한다.
		for (int i = 0; i < MonsterList.Count; i++)
		{
			{
				// 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
				float value = Mathf.Abs((PlayerObject.position - MonsterList[i].GetComponent<Transform>().position).magnitude);

				// 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
				if (min > value || min == null)
				{
					// min 값을 교체하고 
					min = value;

					// 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
					//if (!stateMachine.Player.IsLockOn)
					{
						Target = MonsterList[i].GetComponent<Transform>();

					}
				}
			}

		}

		min = null;
		return true;
	}

	public void LockOn()
	{
		// 타겟이 Player보다 왼쪽에 있는지 오른쪽에 있는지 검사한다.
		if (Target != null)
		{

			stateMachine.Player.IsLockOn = true;

			Vector3 targetPos = TransformPosition(maincamTransform, Target.position);


			// 내적값이 99 보다 작으면 더한다.
			if (xDotResult < lockOnAixsDamp)
			{
				// targetpos로 좌우를 구분해서 돌린다.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - xDotResult));
			}
			if (yDotResult < lockOnAixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - yDotResult));
			}
		}
		else if(MonsterList.Count != 0)
		{
			stateMachine.Player.IsLockOn = false;
			FindTarget();
			stateMachine.Player.IsLockOn = true;
		}
		else
		{
			LockOff();
		}

	}

	public void LockOff()
	{
		// 락온을 해제한다.
		stateMachine.Player.IsLockOn = false;
		Target = null;
	}

	public void SwitchTarget()
	{
		for (int i = MonsterList.Count - 1; i >= 0; i--)
		{
			if (MonsterList[i] == null)
			{
				MonsterList.RemoveAt(i);
			}
		}
		// 몬스터리스트를 거리 순으로 정렬한다.
		SortMosterList();

		// 정렬된 몬스터리스트에서 가장 가까운 몬스터를 Target과 비교한다
		if (MonsterList.Count > 0)
		{
			// 정렬된 가장 가까운몬스터와 target이 일치하면 다음으로 가까운 몬스터를 타겟팅한다.
			if (Target != MonsterList[0].transform)
			{
				Target = MonsterList[0].transform;
			}
			else
			{
				if (MonsterList.Count <= 1)
					return;
				Target = MonsterList[1]?.transform;
			}
		}
	}

	public void SortMosterList()
	{
		// 몬스터리스트를 거리 순으로 정렬한다.
		MonsterList.Sort((x, y) =>
			(PlayerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((PlayerObject.position - y.transform.position).sqrMagnitude));
	}

}
