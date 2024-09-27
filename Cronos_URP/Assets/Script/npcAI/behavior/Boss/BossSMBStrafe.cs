using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BossSMBStrafe : SceneLinkedSMB<BossBehavior>
{
    private float _previusSpeed;
    private float _strafeSpeed;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _strafeSpeed = Random.Range(0, 2);

        _monoBehaviour.controller.animator.SetFloat("strafe_speed", _strafeSpeed);

        _monoBehaviour.controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.controller.UseNavemeshAgentRotation(false);
        _monoBehaviour.controller.navmeshAgent.stoppingDistance = 0;

        if (_monoBehaviour.controller.useAnimatiorSpeed == false)
        {
            _previusSpeed = _monoBehaviour.controller.GetNavemeshAgentSpeed();
            _monoBehaviour.controller.SetNavemeshAgentSpeed(1);
        }

        _monoBehaviour.ResetAiming();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 루트 애니메이션의 위치 실제 위치에 반영
        //_monoBehaviour.transform.position += animator.deltaPosition;

        if (_monoBehaviour.target == null) return;

        // 이동
        if (_strafeSpeed > 0.5f)
        {
            _monoBehaviour.Strafe(true);
        }
        else
        {
            _monoBehaviour.Strafe(false);
        }
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.controller.UseNavemeshAgentRotation(true);

        if (_monoBehaviour.controller.useAnimatiorSpeed == false)
        {
            _monoBehaviour.controller.SetNavemeshAgentSpeed(_previusSpeed);
        }
    }
}
