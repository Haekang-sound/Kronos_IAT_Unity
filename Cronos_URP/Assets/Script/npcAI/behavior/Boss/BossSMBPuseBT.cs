using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSMBPuseBT : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.PlayBT(false);
    }
}
