using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 보스 컷씬 연출을 제어하는 클래스
/// </summary>
public class BossEnterEventController : MonoBehaviour
{
    public GameObject timeline;

    private PlayableDirector _playerbleDirector;
    private bool _sceneSkipped;

    //-----

    void Awake()
    {
        _playerbleDirector = timeline.GetComponent<PlayableDirector>();
    }
    private void Update()
    {
        if (_playerbleDirector.state == PlayState.Playing && Input.GetKeyUp(KeyCode.LeftShift))
        {
            Skip();
        }
    }

    public void Skip()
    {
        if (_sceneSkipped == false)
        {
            _playerbleDirector.time = 41f;
            _sceneSkipped = true;
        }
    }

}
