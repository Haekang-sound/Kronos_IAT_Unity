using UnityEngine;

public class BossSMBRestartBTOnPrexit : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(true);
    }
}
