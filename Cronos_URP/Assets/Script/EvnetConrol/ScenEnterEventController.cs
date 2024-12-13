using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// 첫 번째 씬 시작 연출을 제어하는 클래스입니다.
/// </summary>
public class ScenEnterEventController : MonoBehaviour
{
    public PlayableDirector playerbleDirector;

    private string _key;
    private bool _haskey;
    private bool _sceneSkipped;

    private void Awake()
    {
        playerbleDirector.gameObject.SetActive(false);

        // 키 값 초기화
        _key = SceneManager.GetActiveScene().name + " - timeline";
    }

    void Start()
    {
        // 키 검사
        _haskey = PlayerPrefs.HasKey(_key);

        playerbleDirector.gameObject.SetActive(!_haskey);

        if (_haskey == false)
        {
            playerbleDirector.Play();
            Debug.Log("타임 라인을 재생합니다.");
            PlayerPrefs.SetInt(_key, 1);
        }
    }

    private void Update()
    {
        if (playerbleDirector.state == PlayState.Playing && Input.GetKeyUp(KeyCode.LeftShift))
        {
            Skip();
        }
    }

    private void Skip()
    {
        if (_sceneSkipped == false)
        {
            playerbleDirector.time = 11f;
            _sceneSkipped = true;
        }
    }

    private void OnDisable()
    {
        playerbleDirector.gameObject.SetActive(false);
    }
}
