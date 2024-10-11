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

        // Damaged - ���¿��� �ǰ� ������ ��
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

        _monoBehaviour.rotationSpeed = 8f;
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

            // �ð��� ���� ���� ��

            /// TODO: Player �� �и� ���� ���θ� ��� �޾ƿ� ������ ������ ��
            // Attack - �������� �÷��̾ �־� ���� ��
            if (_monoBehaviour.IsInAttackRange())
            {
                _monoBehaviour.TriggerAttack();
            }
            else if(_monoBehaviour.IsInNormalAttackRange())
            {
                _monoBehaviour.TriggerAttackNormal();
            }
            else if (_monoBehaviour.IsInStrongAttackRange() == false)
            {
                _monoBehaviour.TriggerStrongAttack();
            }
            else
            {
                // Pursuit - ���� �ð��� ���� ���� ��
                _monoBehaviour.StartPursuit();
            }
        }
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.rotationSpeed = 1f;
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