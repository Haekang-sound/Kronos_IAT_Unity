using UnityEngine;

public class BossSMBAvoid : SceneLinkedSMB<BossBehavior>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }

}
