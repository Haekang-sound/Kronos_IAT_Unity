using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBStrafe : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public float minStrafeTime = 2.0f;
    public float maxStrafeTime = 5.0f;

    private float _previusSpeed;
    private float _strafeTime;
    private float _strafeSpeed;
    private float _timer;

    private bool _onRinght;


    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("STRAFE");

        // Damaged - 상태에서 피격 당했을 때
        _monoBehaviour.ResetTriggerDamaged();

        _monoBehaviour.StopPursuit();

        _strafeTime = Random.Range(minStrafeTime, maxStrafeTime);
        _strafeSpeed = Random.Range(-1f, 1f);
        _onRinght = _strafeSpeed > 0;

        if (_onRinght)
        {
            _strafeSpeed = 1f;
        }
        else
        {
            _strafeSpeed = -1f;
        }
        _monoBehaviour.Controller.animator.SetFloat("strafeSpeed", _strafeSpeed);

        ResetTimer();

        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.Controller.UseNavemeshAgentRotation(false);

        if (_monoBehaviour.Controller.useAnimatiorSpeed == false)
        {
            _previusSpeed = _monoBehaviour.Controller.GetNavemeshAgentSpeed();
            _monoBehaviour.Controller.SetNavemeshAgentSpeed(_monoBehaviour.strafeSpeed);
        }

    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;

        if(_onRinght)
        {
            _monoBehaviour.StrafeRight();
        }
        else
        {
            _monoBehaviour.StrafeLeft();
        }

        if (_strafeTime > _timer)
        {
            return;
        }
        else
        {
            ResetTimer();

            // 시간이 종료 됐을 때

            /// TODO: Player 가 패링 가능 여부를 어떻게 받아올 것인지 구현할 것
            // Attack - 범위보다 플레이어가 멀어 졌을 때
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAttack();
            }
            else if (_monoBehaviour.IsInStrongAttackRange() == false)
            {
                _monoBehaviour.TriggerStrongAttack();
            }
            else
            {
                // Pursuit - 상태 시간이 종료 됐을 때
                _monoBehaviour.StartPursuit();
            }
        }
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.Controller.SetFollowNavmeshAgent(false);
        _monoBehaviour.Controller.UseNavemeshAgentRotation(true);

        if (_monoBehaviour.Controller.useAnimatiorSpeed == false)
        {
            _monoBehaviour.Controller.SetNavemeshAgentSpeed(_previusSpeed);
        }

    }

    private void ResetTimer()
    {
        _timer = 0.0f;
    }
}