using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using static Controls;

public class BossrEnterEventController : MonoBehaviour
{
    public GameObject timeline;

    private PlayableDirector _playerbleDirector;
    private bool _sceneSkipped;
    //-----

    void Awake()
    {
        _playerbleDirector = timeline.GetComponent<PlayableDirector>();
    }

    void Start()
    {
        timeline.SetActive(false);
    }

    private void Update()
    {
        if (_playerbleDirector.state == PlayState.Playing && Input.GetKeyUp(KeyCode.LeftShift))
        {
            Skip();
        }
    }

    private void Skip()
    {
        if (_sceneSkipped == false)
        {
            _playerbleDirector.time = 11f;
            _sceneSkipped = true;
        }
    }

}
