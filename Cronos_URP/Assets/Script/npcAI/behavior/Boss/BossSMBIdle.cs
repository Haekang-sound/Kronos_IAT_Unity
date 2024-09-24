using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSMBIdle : SceneLinkedSMB<BossBehavior>
{

    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_monoBehaviour.ResetAiming();
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStatePreExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
