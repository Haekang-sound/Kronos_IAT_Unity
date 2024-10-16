using UnityEngine;

public class BossSMBJumpAttack : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseGravity(false);
    }
}
