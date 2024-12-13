using UnityEngine;

/// <summary>
/// 보스의 경계 상태 전환을 관리하는 클래스입니다.
/// </summary>
public class BossSMBStrafe : SceneLinkedSMB<BossBehavior>
{
    public float minTime = 2f;
    public float maxTime = 5f;
    private float _timer;

    private float _previusSpeed;
    private float _strafeSpeed;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _strafeSpeed = Random.Range(0, 2) == 0 ? -1f : 1f;
        _timer = Random.Range(minTime, maxTime);

        _monoBehaviour.controller.animator.SetFloat("strafe_speed", _strafeSpeed);

        _monoBehaviour.controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.controller.UseNavemeshAgentRotation(false);
        _monoBehaviour.controller.navmeshAgent.stoppingDistance = 0;

        if (_monoBehaviour.controller.useAnimatiorSpeed == false)
        {
            _previusSpeed = _monoBehaviour.controller.GetNavemeshAgentSpeed();
            _monoBehaviour.controller.SetNavemeshAgentSpeed(1f);
        }

        _monoBehaviour.UseGravity(false);
        _monoBehaviour.thisRigidbody.isKinematic = true;
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_monoBehaviour.target == null) return;

        _timer -= _monoBehaviour.GetDeltaTime();

        if (_timer <= 0)
        {
            _monoBehaviour.AnimatorSetTrigger("idle");
        }


        // 이동
        if (_strafeSpeed > 0f)
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
        _monoBehaviour.thisRigidbody.isKinematic = false;

        _monoBehaviour.UseGravity(true);

        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.controller.UseNavemeshAgentRotation(true);

        if (_monoBehaviour.controller.useAnimatiorSpeed == false)
        {
            _monoBehaviour.controller.SetNavemeshAgentSpeed(_previusSpeed);
        }
    }
}
