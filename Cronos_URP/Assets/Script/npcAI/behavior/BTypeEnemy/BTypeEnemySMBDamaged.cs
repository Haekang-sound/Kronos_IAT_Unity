using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTypeEnemySMBDamaged : SceneLinkedSMB<BTypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DAMAGED");
        _monoBehaviour.SetUseKnockback(true);
    }

    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnSLStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.UseBulletTimeScale();
        _monoBehaviour.SetUseKnockback(false);
    }
}