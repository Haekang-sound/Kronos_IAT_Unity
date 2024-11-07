using UnityEngine;

public class BossSMBFireZoneUpgrade : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        _monoBehaviour.BeginAiming();
    }
    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.StopAiming();
    }
}
