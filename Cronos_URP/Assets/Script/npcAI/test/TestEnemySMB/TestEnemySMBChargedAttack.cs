using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySMBChargedAttack : SceneLinkedSMB<TestEnemyBehavior>
{
    protected Vector3 m_AttackPosition;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateEnter(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.controller.SetRotationLerpSeedFast();

        moveForward();
    }

    public override void OnSLTransitionFromStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLTransitionFromStateUpdate(animator, stateInfo, layerIndex);

        _monoBehaviour.StartLookAtTarget();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);

        moveForward();
    }


    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateExit(animator, stateInfo, layerIndex);

        _monoBehaviour.controller.SetFollowNavmeshAgent(true);
    }

    protected void moveForward()
    {
        float animationSpeed = _monoBehaviour.controller.animator.deltaPosition.magnitude;

        // test
        Vector3 direction = (_monoBehaviour.transform.position - _monoBehaviour.controller.target.transform.position).normalized;

        // 새로운 벡터 계산
        Vector3 newDeltaPosition = direction * animationSpeed;

        _monoBehaviour.transform.Translate(newDeltaPosition);
    }
}
