using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSMBOffNavmeshOnEnter : SceneLinkedSMB<BossBehavior>
{
    public override void OnSLStatePostEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.controller.SetFollowNavmeshAgent(false);
    }
}
