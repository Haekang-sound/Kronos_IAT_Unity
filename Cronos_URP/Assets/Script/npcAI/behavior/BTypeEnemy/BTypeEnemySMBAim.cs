using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTypeEnemySMBAim : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public float minStrafeTime = 2.0f;
    public float maxStrafeTime = 5.0f;

    private float _previusSpeed;
    private float _strafeTime;
    private float _strafeSpeed;
    private bool _onRinght;

    private float _timer;

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("AIM");

        _monoBehaviour.Controller.SetFollowNavmeshAgent(true);
        _monoBehaviour.Controller.UseNavemeshAgentRotation(false);

        // Damaged - ���¿��� �ǰ� ������ ��
        _monoBehaviour.ResetTriggerDamaged();

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

        if (_monoBehaviour.Controller.useAnimatiorSpeed == false)
        {
            _previusSpeed = _monoBehaviour.Controller.GetNavemeshAgentSpeed();
            _monoBehaviour.Controller.SetNavemeshAgentSpeed(_monoBehaviour.strafeSpeed);
        }
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timer += Time.deltaTime;

        if (_onRinght)
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

            _monoBehaviour.FindTarget();

            // ATTACK - Ÿ���� ���� ��Ÿ� �ȿ� ���� ��
            if (_monoBehaviour.CurrentTarget != null)
            {
                if (_monoBehaviour.IsInAttackRange())
                {
                    _monoBehaviour.TriggerAttack();
                }
                else
                {
                    _monoBehaviour.TriggerPursuit();
                }
            }
            // IDLE - Ÿ���� ã�� �� ���� ��
            else
            {
                _monoBehaviour.TriggerIdle();
            }
        }
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.Controller.UseNavemeshAgentRotation(true);
        _monoBehaviour.Controller.SetFollowNavmeshAgent(false);
    }

    private void ResetTimer()
    {
        _timer = 0.0f;
    }
}