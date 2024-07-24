using System.Collections;
using UnityEngine;

public class GameObjectTeleporter : MonoBehaviour
{
	public static GameObjectTeleporter Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<GameObjectTeleporter>();

			if (instance != null)
				return instance;

			GameObject gameObjectTeleporter = new GameObject("GameObjectTeleporter");
			instance = gameObjectTeleporter.AddComponent<GameObjectTeleporter>();

			return instance;
		}
	}

	public static bool Transitioning
	{
		get { return Instance.m_transitioning; }
	}

	protected static GameObjectTeleporter instance;

	//protected PlayerInput m_playerInput;
	protected bool m_transitioning;

	void Awake()
	{
		if (Instance != this)
        {
			Destroy(gameObject);
            return;
        }

		DontDestroyOnLoad(gameObject);

		//m_playerInput = FindObjectOfType<PlayerInput>();
	}

	public static void Teleport(TransitionPoint transitionPoint)
	{
		Transform destinationTransform = Instance.GetDestination().transform;
		Instance.StartCoroutine(Instance.Transition(transitionPoint.transitioningGameObject, true, destinationTransform.position, true));
	}

	public static void Teleport(GameObject transitioningGameObject, Transform destination)
	{
		Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destination.position, false));
	}

	public static void Teleport(GameObject transitioningGameObject, Vector3 destinationPosition)
	{
		Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destinationPosition, false));
	}

	protected IEnumerator Transition(GameObject transitioningGameObject, bool releaseControl, Vector3 destinationPosition, bool fade)
	{
		m_transitioning = true;

		//if (releaseControl)
		//{
		//	if (m_playerInput == null)
		//		m_playerInput = FindObjectOfType<PlayerInput>();
		//	m_playerInput.ReleaseControl();
		//}

		if (fade)
			yield return StartCoroutine(ScreenFader.FadeSceneOut());

		transitioningGameObject.transform.position = destinationPosition;

		if (fade)
			yield return StartCoroutine(ScreenFader.FadeSceneIn());

		//if (releaseControl)
		//{
		//	m_playerInput.GainControl();
		//}

		m_transitioning = false;
	}

	protected SceneTransitionDestination GetDestination()
	{
		SceneTransitionDestination entrance = FindObjectOfType<SceneTransitionDestination>();
		if (entrance != null)
		{
			return entrance;
		}

		Debug.LogWarning("SceneTransitionDestination 를 찾을 수 없습니다.");
		return null;
	}
}