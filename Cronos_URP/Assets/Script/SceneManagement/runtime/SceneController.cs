using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class SceneController : MonoBehaviour
{
    // �̱��� ��ü �Դϴ�. 
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            // �ν��Ͻ��� ���ٸ� ���� ����â���� �˻��ؼ� ������.
            instance = FindObjectOfType<SceneController>();

            if (instance != null)
            {
                return instance;
            }

            // �ν��Ͻ��� ���ٸ� ���� ����
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

        SaveLoadManager.Instance.SaveSceneData();

        //if (m_playerInput)
        //{
        //    m_playerInput.ReleaseControl();
        //}
        
        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));

        //PersistentDataManager.ClearPersisters();

        /// �� �ε� �� �׽�Ʈ��
        //yield return SceneManager.LoadSceneAsync(newSceneName);
        yield return SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.LoadSceneCoroutine(newSceneName));

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

        yield return StartCoroutine(ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading));

        //if (m_playerInput)
        //{
        //    m_playerInput.GainControl();
        //}

        //SaveLoadManager.Instance.LoadSceneData();

        m_transitioning = false;
    }

    protected SceneTransitionDestination GetDestination()
    {
        SceneTransitionDestination entrances = FindObjectOfType<SceneTransitionDestination>();
        if (entrances != null) 
        {
            return entrances;
        }

        Debug.LogWarning("SceneTransitionDestination ������Ʈ�� ���� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        return null;
    }

    protected void SetEnteringGameObjectLocation(SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("���� ��ġ(SceneTransitionDestination) �Է��� �������� �ʾҽ��ϴ�.");
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
            Debug.LogWarning("����� ����(SceneTransitionDestination)�� �������� �ʾҽ��ϴ�.");
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
