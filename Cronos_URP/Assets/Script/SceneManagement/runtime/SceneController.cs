using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 씬 전환 및 관리 기능을 담당하는 싱글톤 클래스입니다.
/// 씬 로딩, 전환, 재시작 및 텔레포트와 관련된 기능을 제공합니다.
/// </summary>
public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<SceneController>();

            if (instance != null)
            {
                return instance;
            }

            Create();

            return instance;
        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_transitioning; }
    }

    protected static SceneController instance;  // 싱글톤 인스턴스
    public SceneTransitionDestination initialSceneTransitionDestination;  // 초기 씬 전환 위치
    protected Scene m_currentZoneScene;  // 현재 씬 정보
    protected bool m_transitioning;  // 씬 전환 중 여부

    /// <summary>
    /// 씬 컨트롤러 인스턴스를 생성합니다.
    /// </summary>
    public static SceneController Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        instance = sceneControllerGameObject.AddComponent<SceneController>();

        return instance;
    }

    void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(1920, 1080, true);  // 해상도 설정
#endif

        if (Instance != this)
        {
            Destroy(gameObject);  // 다른 인스턴스가 있으면 객체를 파괴
            return;
        }

        DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정

        if (initialSceneTransitionDestination != null)
        {
            SetEnteringGameObjectLocation(initialSceneTransitionDestination);  // 초기 위치 설정
            ScreenFader.SetAlpha(1f);
            StartCoroutine(ScreenFader.FadeSceneIn());  // 씬 페이드 인
            initialSceneTransitionDestination.OnReachDestination.Invoke();  // 목표 도달 이벤트 호출
        }
        else
        {
            m_currentZoneScene = SceneManager.GetActiveScene();  // 현재 씬 저장
        }
    }

    /// <summary>
    /// 현재 씬을 다시 시작합니다.
    /// </summary>
    public static void RestartZone()
    {
        Instance.StartCoroutine(Instance.Transition(Instance.m_currentZoneScene.name));
    }

    /// <summary>
    /// 지정된 시간 후 현재 씬을 다시 시작합니다.
    /// </summary>
    public static void RestartZoneWithDelay(float delay)
    {
        Instance.StartCoroutine(CallWithDelay(delay, RestartZone));
    }

    /// <summary>
    /// 새 씬으로 전환합니다.
    /// </summary>
    public static void TransitionToScene(TransitionPoint transitionPoint)
    {
        Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName));
    }

    /// <summary>
    /// 지정된 씬으로 전환합니다.
    /// </summary>
    public static void TransitionToScene(string newSceneName)
    {
        Instance.StartCoroutine(Instance.Transition(newSceneName));
    }

    /// <summary>
    /// 씬 전환을 처리하는 코루틴입니다.
    /// </summary>
    protected IEnumerator Transition(string newSceneName, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
    {
        m_transitioning = true;

        SaveLoadManager.Instance.SaveSceneData();  // 씬 데이터 저장

        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));  // 씬 페이드 아웃

        yield return SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.LoadSceneCoroutine(newSceneName));  // 씬 로딩

        SceneTransitionDestination entrance = GetDestination();  // 목표 위치 찾기

        SetEnteringGameObjectLocation(entrance);  // 게임 오브젝트 위치 설정
        SetupNewScene(transitionType, entrance);  // 씬 설정

        if (entrance != null)
        {
            entrance.OnReachDestination.Invoke();  // 목표 도달 이벤트 호출
        }

        yield return StartCoroutine(ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading));  // 씬 페이드 인

        m_transitioning = false;
    }

    /// <summary>
    /// 씬 전환 목표인 SceneTransitionDestination을 찾습니다.
    /// </summary>
    protected SceneTransitionDestination GetDestination()
    {
        SceneTransitionDestination entrances = FindObjectOfType<SceneTransitionDestination>();
        if (entrances != null)
        {
            return entrances;
        }

        Debug.LogWarning("SceneTransitionDestination 컴포넌트를 가진 게임 오브젝트를 찾을 수 없습니다.");
        return null;
    }

    /// <summary>
    /// 게임 오브젝트의 위치를 SceneTransitionDestination의 위치로 설정합니다.
    /// </summary>
    protected void SetEnteringGameObjectLocation(SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("시작 위치(SceneTransitionDestination) 입력이 설정되지 않았습니다.");
            return;
        }
        Transform entranceLocation = entrance.transform;
        Transform enteringTransform = entrance.transitioningGameObject.transform;
        enteringTransform.position = entranceLocation.position;
        enteringTransform.rotation = entranceLocation.rotation;
    }

    /// <summary>
    /// 새 씬을 설정합니다.
    /// </summary>
    protected void SetupNewScene(TransitionPoint.TransitionType transitionType, SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("재시작 정보(SceneTransitionDestination)가 설정되지 않았습니다.");
            return;
        }

        if (transitionType == TransitionPoint.TransitionType.DifferentZone)
        {
            SetZoneStart(entrance);  // 다른 구역일 경우, 구역 시작 설정
        }
    }

    /// <summary>
    /// 새로운 구역의 시작을 설정합니다.
    /// </summary>
    protected void SetZoneStart(SceneTransitionDestination entrance)
    {
        m_currentZoneScene = entrance.gameObject.scene;
    }

    static IEnumerator CallWithDelay<T>(float delay, Action<T> call, T parameter)
    {
        yield return new WaitForSeconds(delay);
        call(parameter);
    }

    static IEnumerator CallWithDelay(float delay, Action call)
    {
        yield return new WaitForSeconds(delay);
        call();
    }
}
