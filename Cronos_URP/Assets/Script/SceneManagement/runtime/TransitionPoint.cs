using UnityEngine;

/// <summary>
/// 동일한 씬 내 텔레포트를 관리합니다.
/// 이 클래스는 특정 조건에 따라 게임 오브젝트를 전환하거나 이동시키는 역할을 합니다.
/// </summary>
public class TransitionPoint : MonoBehaviour
{
	/// <summary>
	/// 씬 전환 유형
	/// </summary>
	public enum TransitionType
	{
		DifferentZone,
		DifferentNonGameplayScene,
		SameScene,
	}

	/// <summary>
	/// 씬 전환이 발생하는 시점 정의
	/// </summary>
	public enum TransitionWhen
	{
		OnTriggerEnter,
		ExternalCall,
	}

	[Tooltip("씬 전환이 될때 같이 이동될 게임 오브젝트입니다. (ex. 플레이어)")]
	public GameObject transitioningGameObject;
	[Tooltip("전환이 이 장면 내에서 이루어질지, 다른 영역으로 전환될지, 아니면 게임플레이가 아닌 장면으로 전환될지 결정합니다.")]
	public TransitionType transitionType;
	[SceneName]
	public string newSceneName;
	[Tooltip("이 씬에서 전환되는 게임 오브젝트가 텔레포트될 목적지입니다.")]
	public TransitionPoint destinationTransform;
	[Tooltip("씬 전환이 시작되도록 트리거해야 하는 항목.")]
	public TransitionWhen transitionWhen;

	bool m_transitioningGameObjectPresent;

	void Start()
	{
		if (transitionWhen == TransitionWhen.ExternalCall)
		{
			m_transitioningGameObjectPresent = true;
		}

	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("트랜지션 포인트 트리거");
		if (other.gameObject == transitioningGameObject)
		{
			m_transitioningGameObjectPresent = true;

			if (transitionWhen == TransitionWhen.OnTriggerEnter)
			{
				TransitionInternal();
			}
			else
			{
				EffectManager.Instance.CreateGuardFX();
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("트랜지션 포인트 나옴");
		if (other.gameObject == transitioningGameObject)
		{
			m_transitioningGameObjectPresent = false;
		}
	}

	protected void TransitionInternal()
	{

		if (transitionType == TransitionType.SameScene)
		{
			GameObjectTeleporter.Teleport(transitioningGameObject, destinationTransform.transform);
		}
		else
		{
			SceneController.TransitionToScene(this);
		}
	}

	public void Transition()
	{
		if (m_transitioningGameObjectPresent)
		{
			if (transitionWhen == TransitionWhen.ExternalCall)
			{
				TransitionInternal();
			}
		}
	}
}
