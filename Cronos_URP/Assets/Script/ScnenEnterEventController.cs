using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ScnenEnterEventController : MonoBehaviour
{
    public PlayableDirector playerbleDirector;

    [SerializeField]
    private string _key;
    [SerializeField]
    private bool _haskey;

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

    private void OnDisable()
    {
        playerbleDirector.gameObject.SetActive(false);
    }
}
