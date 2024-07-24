using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATypeEnemySMBDeath : SceneLinkedSMB<ATypeEnemyBehavior>
{
    public override void OnSLStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _monoBehaviour.ChangeDebugText("DEATH");
    }
}