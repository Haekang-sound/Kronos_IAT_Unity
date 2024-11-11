using UnityEngine;

public class BossSMBFireZone : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        _monoBehaviour.BeginAiming();
    }
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }
}
