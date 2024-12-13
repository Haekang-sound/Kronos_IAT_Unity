using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 오브젝트의 텔레포트 기능을 관리하는 클래스입니다.
/// </summary>
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

            // GameObjectTeleporter 객체가 없으면 새로 생성
            GameObject gameObjectTeleporter = new GameObject("GameObjectTeleporter");
            instance = gameObjectTeleporter.AddComponent<GameObjectTeleporter>();

            return instance;
        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_transitioning; }
    }

    protected static GameObjectTeleporter instance;  // 싱글톤 인스턴스
    protected bool m_transitioning;  // 텔레포트 진행 중 상태

    void Awake()
    {
        // 이미 싱글톤 인스턴스가 있으면 객체를 파괴하고, 그렇지 않으면 현재 객체를 유지
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정
    }

    /// <summary>
    /// 지정된 TransitionPoint로 텔레포트
    /// </summary>
    public static void Teleport(TransitionPoint transitionPoint)
    {
        Transform destinationTransform = Instance.GetDestination().transform;
        Instance.StartCoroutine(Instance.Transition(transitionPoint.transitioningGameObject, true, destinationTransform.position, true));
    }

    /// <summary>
    /// 게임 오브젝트를 지정된 Transform 위치로 텔레포트
    /// </summary>
    public static void Teleport(GameObject transitioningGameObject, Transform destination)
    {
        Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destination.position, false));
    }

    /// <summary>
    /// 게임 오브젝트를 지정된 위치로 텔레포트
    /// </summary>
    public static void Teleport(GameObject transitioningGameObject, Vector3 destinationPosition)
    {
        Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destinationPosition, false));
    }

    /// <summary>
    /// 텔레포트 진행을 처리하는 코루틴
    /// </summary>
    protected IEnumerator Transition(GameObject transitioningGameObject, bool releaseControl, Vector3 destinationPosition, bool fade)
    {
        m_transitioning = true;

        if (fade)
        {
            yield return StartCoroutine(ScreenFader.FadeSceneOut());  // 씬 페이드 아웃
        }

        transitioningGameObject.transform.position = destinationPosition;  // 위치 이동

        if (fade)
        {
            yield return StartCoroutine(ScreenFader.FadeSceneIn());  // 씬 페이드 인
        }

        m_transitioning = false;
    }

    /// <summary>
    /// 텔레포트 목표 위치인 SceneTransitionDestination 객체를 찾음
    /// </summary>
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
