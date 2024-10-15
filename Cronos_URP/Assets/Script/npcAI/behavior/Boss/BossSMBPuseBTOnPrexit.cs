using UnityEngine;

public class BossSMBPuseBTOnPrexit : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(false);
    }
}