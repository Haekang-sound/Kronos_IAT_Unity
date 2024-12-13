using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TestEnemySMBAttack : SceneLinkedSMB<TestEnemyBehavior>
{
    protected Vector3 m_AttackPosition;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.controller.SetRotationLerpSeedFast();
    }

    public override void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.StartLookAtTarget();
    }


    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(true);
    }
}
