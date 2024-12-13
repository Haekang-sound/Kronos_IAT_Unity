using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSMBOnGroggy : SceneLinkedSMB<BossBehavior>
{

    public float groggyTime = 8f;
    private float _timer;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer = 0f;
        _monoBehaviour.thisRigidbody.isKinematic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;
        if (_timer > groggyTime)
        {
            _monoBehaviour.EndGroggy();
        }
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.thisRigidbody.isKinematic = false;
    }
}
