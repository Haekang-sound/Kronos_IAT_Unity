using UnityEngine;

public class BossSMBPuseBTOnUpdate : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(false);
    }
}
