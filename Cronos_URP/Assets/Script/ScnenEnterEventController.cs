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

    private void OnDisable()
    {
        playerbleDirector.gameObject.SetActive(false);
    }
}
