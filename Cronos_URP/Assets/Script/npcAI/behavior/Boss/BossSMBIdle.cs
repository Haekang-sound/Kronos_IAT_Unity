using UnityEngine;

public class BossSMBIdle : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseGravity(true);

        _monoBehaviour.StopAiming();

        _monoBehaviour.EndAttack();
        _monoBehaviour.EndImpactAttack();
        _monoBehaviour.EndSoulderAttack();
    }
}
