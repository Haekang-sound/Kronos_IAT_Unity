using UnityEngine;

/// <summary>
/// ATypeEnem�� ��� ���� ��ȯ�� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class ATypeEnemySMBDeath : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DEATH");
    }
}