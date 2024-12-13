using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySMBReturn : SceneLinkedSMB<TestEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.WalkBackToBase();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.FindTarget();

        if (_monoBehaviour.Target != null)
            _monoBehaviour.StartPursuit(); // if the player got back in our vision range, resume pursuit!
        else
            _monoBehaviour.WalkBackToBase();
    }
}
