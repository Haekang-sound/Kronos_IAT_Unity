using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

/// <summary>
/// 자동조준과 lockOn을 
/// 동작 시키는 클래스
/// 
/// ohk    v1
/// </summary>
public class AutoTargetting : MonoBehaviour
{
	// 싱글턴을 위한 구간
	private static AutoTargetting _instance;
	public static AutoTargetting GetInstance() { return _instance; }

	private PlayerStateMachine _stateMachine;

	// 작동범위를 위한 충돌체
	public Collider sphere;

	// 카메라 정보를 받아오는 변수
	public CinemachineVirtualCamera playerCamera;
	public CinemachinePOV cinemachinePOV;
	private Transform _maincamTransform;

	// 카메라 이동속도
	public float horizontalSpeed = 10.0f;
	public float verticalSpeed = 3f;

	// 타겟을 조준하기위한 플레이어와 타겟
	public GameObject player;
	public Transform target = null;
	public Transform playerObject;

	// 조준상태를 확인하는 변수
	public float lockOnAixsDamp = 0.99f;
	public float autoTargettingAixsDamp = 0.99f;
	public float exitValue;
	private Vector3 _direction;
	private float _xDotResult;
	private float _yDotResult;
	private bool _isFacing = false;
	public bool isTargetting = false;

	[SerializeField] private List<GameObject> _monsterList;
	private float? _min = null;

	private void Awake()
	{
		_instance = this;
		_monsterList = new List<GameObject>();
	}

	public void OnEnable()
	{
		_stateMachine = player.GetComponent<PlayerStateMachine>();

		_maincamTransform = Camera.main.transform;

		if (_maincamTransform != null)
		{
			cinemachinePOV = playerCamera.GetCinemachineComponent<CinemachinePOV>();
		}
	}

	private void OnDisable()
	{
		target = null;
	}

	private void FixedUpdate()
	{
		// Player가 몬스터 방향으로 몸을 돌린다.
		if ((isTargetting || _stateMachine.Player.IsLockOn || _isFacing)
			&& _stateMachine.GetState().ToString() != "PlayerParryState")
		{
			if (_direction.magnitude != 0f && _stateMachine.InputReader.moveComposite.magnitude == 0f)
			{
				_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(_direction.normalized), 0.2f));
			}
		}
	}

	/// 콜라이더에 몬스터가 들어오면 리스트에 추가한다.
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			_monsterList.Add(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}

	}
	/// 콜라이더에서 몬스터가 나가면 리스트에서 제거한다.
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			_monsterList.Remove(FindChildRecursive(other.gameObject.transform, "LockOnTarget").gameObject);
		}
	}

	void Update()
	{
		// Player가 바라볼 방향을 정한다.
		if (target != null)
		{
			_direction = target.position - playerObject.position;
			_direction.y = 0;
		}

		_xDotResult = Mathf.Abs(Vector3.Dot(_maincamTransform.right, Vector3.Cross(Vector3.up, _direction.normalized).normalized));
		_yDotResult = Mathf.Abs(Vector3.Dot(_maincamTransform.up, Vector3.Cross(Vector3.right, _direction.normalized).normalized));

		if (_stateMachine.Player.IsLockOn)
		{
			LockOn();
		}
	}

	public Transform GetTarget()
	{
		return target;
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
		if (Mathf.Abs(Vector3.Dot(_stateMachine.transform.forward, _direction.normalized)) > 0.9f)
		{
			_isFacing = false;
		}
	}

	private void StopAutoTargetting()
	{
		isTargetting = false;
	}

	public void AutoTargeting()
	{
		if (target == null)
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
		else
		{
			isTargetting = true;
			StartCoroutine(AutoTarget());
		}
	}

	private IEnumerator AutoTarget()
	{
		isTargetting = true;
		_isFacing = true;

		while (_isFacing)
		{
			FacingTarget();
			yield return null;
		}

		while (isTargetting)
		{
			if (target == null)
			{
				StopAutoTargetting();
			}
			else
			{
				Vector3 targetPos = TransformPosition(_maincamTransform, target.position);

				// 내적값이 0.99 보다 작으면 더한다.
				if (_xDotResult < autoTargettingAixsDamp)
				{
					if (_xDotResult > exitValue && _stateMachine.InputReader.moveComposite.magnitude != 0f)
					{
						StopAutoTargetting();
					}
					TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1 - _xDotResult));
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
		cinemachinePOV.m_HorizontalAxis.Value += value;
	}

	private void TurnCamVertical(float value)
	{
		cinemachinePOV.m_VerticalAxis.Value -= value;
	}

	// 특정 포지션을 특정 트렌스폼에서 바라본다.
	private Vector3 TransformPosition(Transform transform, Vector3 worldPosition)
	{
		return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);
	}

	public bool FindTarget()
	{
		// 몬스터리스트가 없거나 사이즈가 0이라면 false
		if (_monsterList == null || _monsterList.Count == 0)
		{
			target = null;
			return false;
		}
		if (_stateMachine.Player.IsLockOn)
		{
			return true;
		}

		// 현재 몬스터 목록을 순회한다.
		for (int i = _monsterList.Count - 1; i >= 0; i--)
		{
			if (_monsterList[i] == null || !_monsterList[i].activeSelf)
			{
				_monsterList.RemoveAt(i);
			}
		}

		// 몬스터리스트를 거리 순으로 정렬한다.
		SortMosterList();

		// 현재 몬스터 목록을 순회한다.
		for (int i = 0; i < _monsterList.Count; i++)
		{
			{
				// 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
				float value = Mathf.Abs((playerObject.position - _monsterList[i].GetComponent<Transform>().position).magnitude);

				// 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
				if (_min > value || _min == null)
				{
					// min 값을 교체하고 
					_min = value;

					// 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
					//if (!stateMachine.Player.IsLockOn)
					{
						target = _monsterList[i].GetComponent<Transform>();

					}
				}
			}

		}

		_min = null;
		return true;
	}

	public void LockOn()
	{
		// 타겟이 Player보다 왼쪽에 있는지 오른쪽에 있는지 검사한다.
		if (target != null)
		{

			_stateMachine.Player.IsLockOn = true;

			Vector3 targetPos = TransformPosition(_maincamTransform, target.position);


			// 내적값이 99 보다 작으면 더한다.
			if (_xDotResult < lockOnAixsDamp)
			{
				// targetpos로 좌우를 구분해서 돌린다.
				TurnCamHorizontal(horizontalSpeed * Time.deltaTime * (targetPos.x / math.abs(targetPos.x)) * (1f - _xDotResult));
			}
			if (_yDotResult < lockOnAixsDamp)
			{
				TurnCamVertical(verticalSpeed * Time.deltaTime * (targetPos.y / math.abs(targetPos.y)) * (1f - _yDotResult));
			}
		}
		else if (_monsterList.Count != 0)
		{
			_stateMachine.Player.IsLockOn = false;
			FindTarget();
			_stateMachine.Player.IsLockOn = true;
		}
		else
		{
			LockOff();
		}

	}

	public void LockOff()
	{
		// 락온을 해제한다.
		_stateMachine.Player.IsLockOn = false;
		target = null;
	}

	public void SwitchTarget()
	{
		for (int i = _monsterList.Count - 1; i >= 0; i--)
		{
			if (_monsterList[i] == null)
			{
				_monsterList.RemoveAt(i);
			}
		}
		// 몬스터리스트를 거리 순으로 정렬한다.
		SortMosterList();

		// 정렬된 몬스터리스트에서 가장 가까운 몬스터를 Target과 비교한다
		if (_monsterList.Count > 0)
		{
			// 정렬된 가장 가까운몬스터와 target이 일치하면 다음으로 가까운 몬스터를 타겟팅한다.
			if (target != _monsterList[0].transform)
			{
				target = _monsterList[0].transform;
			}
			else
			{
				if (_monsterList.Count <= 1)
					return;
				target = _monsterList[1]?.transform;
			}
		}
	}

	public void SortMosterList()
	{
		// 몬스터리스트를 거리 순으로 정렬한다.
		_monsterList.Sort((x, y) =>
			(playerObject.position - x.transform.position).sqrMagnitude
			.CompareTo((playerObject.position - y.transform.position).sqrMagnitude));
	}

}
