using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class SceneController : MonoBehaviour
{
    // 싱글턴 객체 입니다. 
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            // 인스턴스가 없다면 계층 구조창에서 검색해서 가져옴.
            instance = FindObjectOfType<SceneController>();

            if (instance != null)
            {
                return instance;
            }

            // 인스턴스가 없다면 직접 생성
            Create();

            return instance;
        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_transitioning; }
    }

    protected static SceneController instance;

    public SceneTransitionDestination initialSceneTransitionDestination;

    protected Scene m_currentZoneScene;
    //protected PlayerInput m_playerInput;
    protected bool m_transitioning;

    public static SceneController Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        instance = sceneControllerGameObject.AddComponent<SceneController>();

        return instance;
    }

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //m_playerInput = FindObjectOfType<PlayerInput>();

        if (initialSceneTransitionDestination != null)
        {
            SetEnteringGameObjectLocation(initialSceneTransitionDestination);
            ScreenFader.SetAlpha(1f);
            StartCoroutine(ScreenFader.FadeSceneIn());
            initialSceneTransitionDestination.OnReachDestination.Invoke();
        }
        else
        {
            m_currentZoneScene = SceneManager.GetActiveScene();
        }
    }

    public static void RestartZone()
    {
        Instance.StartCoroutine(Instance.Transition(Instance.m_currentZoneScene.name));
    }

    public static void RestartZoneWithDelay(float delay)
    {
        Instance.StartCoroutine(CallWithDelay(delay, RestartZone));
    }

    public static void TransitionToScene(TransitionPoint transitionPoint)
    {
        Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName));
    }

    public static void TransitionToScene(string newSceneName)
    {
        Instance.StartCoroutine(Instance.Transition(newSceneName));
    }

    protected IEnumerator Transition(string newSceneName, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
    {
        m_transitioning = true;
        //PersistentDataManager.SaveAllData();

        //if (m_playerInput == null)
        //{
        //    m_playerInput = FindObjectOfType<PlayerInput>();
        //}

        //if (m_playerInput)
        //{
        //    m_playerInput.ReleaseControl();
        //}
        
        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));

        //PersistentDataManager.ClearPersisters();

        yield return SceneManager.LoadSceneAsync(newSceneName);

        //m_playerInput = FindObjectOfType<PlayerInput>();

        //if (m_playerInput)
        //{
        //    m_playerInput.ReleaseControl();
        //}
        //PersistentDataManager.LoadAllData();

        SceneTransitionDestination entrance = GetDestination();

        SetEnteringGameObjectLocation(entrance);

        SetupNewScene(transitionType, entrance);

        if (entrance != null)
        {
            entrance.OnReachDestination.Invoke();
        }

        yield return StartCoroutine(ScreenFader.FadeSceneIn());

        //if (m_playerInput)
        //{
        //    m_playerInput.GainControl();
        //}

        m_transitioning = false;
    }

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

    protected void SetupNewScene(TransitionPoint.TransitionType transitionType, SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("재시작 정보(SceneTransitionDestination)가 설정되지 않았습니다.");
            return;
        }

        if (transitionType == TransitionPoint.TransitionType.DifferentZone)
        {
            SetZoneStart(entrance);
        }
    }

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
