using UnityEngine;

/// <summary>
/// BTypeEnem�� ��� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class BTypeEnemySMBDeath : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DEATH");
    }
}