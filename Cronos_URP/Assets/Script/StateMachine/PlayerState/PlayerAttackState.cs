using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackState : PlayerBaseState
{
    AnimatorStateInfo stateInfo;

    private readonly int AttackHash1 = Animator.StringToHash("Combo_01_1");
    private readonly int AttackHash2 = Animator.StringToHash("Combo_01_2");
    private readonly int AttackHash3 = Animator.StringToHash("Combo_01_3");
    private readonly int AttackHash4 = Animator.StringToHash("Combo_01_4");
    private const float CrossFadeDuration = 0.1f;

    public float startNormalizedTime = 0.3f;    // ���� ����
    public float endNormalizedTime = 0.99f;     // ���� ����

    List<int> comboAttack;

    int comboStack = 0;

    private bool nextCombo = false;
    private bool nextJab = false;
    private bool nextPunch = false;

    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        // �޺� �ؽ�����Ʈ
        comboAttack = new List<int>();
        // �ִϸ��̼� �ؽ����� �߰��Ѵ�.
        comboAttack.Add(AttackHash1);
        comboAttack.Add(AttackHash2);
        comboAttack.Add(AttackHash3);
        comboAttack.Add(AttackHash4);

        stateMachine.Animator.Rebind();
        stateMachine.Animator.CrossFadeInFixedTime(comboAttack[comboStack], CrossFadeDuration);
        // ���� �ִϸ��̼� ������ �޾ƿ´�

        stateMachine.InputReader.onLAttackStart += ReadyNextCombo;
		stateMachine.InputReader.onRAttackStart += SwitchToDefanceState;
	}
    public override void Tick()
    {
        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        // ��Ʈ��ž Ÿ�̹�
        if (stateInfo.normalizedTime >= stateMachine.Player.stopTiming)
        {
            stateMachine.HitStop.isHit = true;
            stateMachine.HitStop.StartCoroutine(stateMachine.HitStop.HitStopCoroutine());
        }
        float testtime = 0.99f;
        switch (comboStack)
        {
            case 1:
                testtime = 0.7f;
                break;
            case 2:
                testtime = 0.6f;
                break;
            case 3:
                testtime = 0.7f;
                break;
            default:
                testtime = 0.99f;
                break;
        }
        if (stateInfo.normalizedTime >= testtime && stateInfo.normalizedTime <= 1.1f)
        {
            // ���� �޺������� �����Ǿ��ִٸ�
            if (nextCombo)
            {
                // ���ο� �޺������� �����Ѵ�.
                NextCombo();

            }
        }
        // �ִϸ��̼��� ����ǰ�
        if (stateInfo.normalizedTime >= 1.0f && stateInfo.normalizedTime <= 1.1f)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }

    }
    public override void Exit()
    {
		stateMachine.InputReader.onRAttackStart -= SwitchToDefanceState;
		stateMachine.InputReader.onLAttackStart -= ReadyNextCombo;
    }


    public void ReadyNextCombo()
    {
        // �� �Լ��� ȣ��Ǿ�����
        // ���������� �����Ǿ��ִٸ� �����Ѵ�.
        if (nextCombo == true || comboStack == 3)
        {
            return;
        }

        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= startNormalizedTime && stateInfo.normalizedTime <= endNormalizedTime)
        {
            // ���� �޺������� true�� �ϰ�
            nextCombo = true;

            // �޺������� ����Ǿ��ִ� ������� �۴ٸ�
            if (comboStack < comboAttack.Count - 1)
            {
                comboStack++;
            }

        }

    }
    // ���ο� �޺� �ִϸ��̼��� �����Ѵ�.
    public void NextCombo()
    {
        stateMachine.Animator.Rebind();
        float normalizedStartTime = 0.0f;
        switch (comboStack)
        {
            //             case 1:
            //                 normalizedStartTime = 0.1f; // �ִϸ��̼��� 0.3�� �������� ����
            //                 break;
            case 2:
                normalizedStartTime = 0.2f; // �ִϸ��̼��� 0.3�� �������� ����
                break;
            case 3:
                normalizedStartTime = 0.3f; // �ִϸ��̼��� 0.3�� �������� ����
                break;
        }

        stateMachine.Animator.CrossFadeInFixedTime(comboAttack[comboStack], 0.1f, -1, normalizedStartTime);
        
        nextCombo = false;
    }


	private void SwitchToDefanceState()
	{
		stateMachine.SwitchState(new PlayerDefenceState(stateMachine));
	}


}
