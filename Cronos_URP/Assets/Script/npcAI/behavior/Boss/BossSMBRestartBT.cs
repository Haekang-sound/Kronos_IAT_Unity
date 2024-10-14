using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSMBRestartBT : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(true);
    }
}
