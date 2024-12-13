using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// ù ��° �� ���� ������ �����ϴ� Ŭ�����Դϴ�.
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

        // Ű �� �ʱ�ȭ
        _key = SceneManager.GetActiveScene().name + " - timeline";
    }

    void Start()
    {
        // Ű �˻�
        _haskey = PlayerPrefs.HasKey(_key);

        playerbleDirector.gameObject.SetActive(!_haskey);

        if (_haskey == false)
        {
            playerbleDirector.Play();
            Debug.Log("Ÿ�� ������ ����մϴ�.");
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
